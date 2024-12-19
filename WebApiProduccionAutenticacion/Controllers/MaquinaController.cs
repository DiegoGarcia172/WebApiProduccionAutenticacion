using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text;
using WebApiProduccionAutenticacion.Data;
using WebApiProduccionAutenticacion.Models;

namespace WebApiProduccionAutenticacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaquinaController : Controller
    {
        private readonly ProduccionDbContext _context;

        public MaquinaController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maquina>>> GetMaquina()
        {
            return await _context.Maquina.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Maquina>> GetMaquinaById(int id)
        {
            var Maquina = await _context.Maquina.FindAsync(id);

            if (Maquina == null)
            {
                return NotFound();

            }

            return Ok(Maquina);
        }
        [HttpPost]
        public async Task<ActionResult<Maquina>> CrearMaquina([FromBody] Maquina maquina)
        {
            if (maquina == null)
            {
                return BadRequest();
            }
            maquina.ID = 0;
            _context.Maquina.Add(maquina);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMaquinaById), new { id = maquina.ID }, maquina);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMaquina(int id, [FromBody] Maquina maquinaActualizada)
        {
            if (id != maquinaActualizada.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var maquinaExistente = await _context.Maquina.FindAsync(id);

            if (maquinaExistente == null)
            {
                return NotFound($"La máquina con ID {id} no existe.");
            }
            maquinaExistente.Nombre = maquinaActualizada.Nombre;
            maquinaExistente.Estado = maquinaActualizada.Estado;
            maquinaExistente.Tipo = maquinaActualizada.Tipo;
            maquinaExistente.Modelo = maquinaActualizada.Modelo;
            maquinaExistente.id_proceso = maquinaActualizada.id_proceso;
            maquinaExistente.id_ordenproduccion = maquinaActualizada.id_ordenproduccion;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"La máquina con ID {id} fue actualizada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar la máquina: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMaquina(int id)
        {
            var maquina = await _context.Maquina.FindAsync(id);

            if (maquina == null)
            {
                return NotFound($"La máquina con ID {id} no existe.");
            }

            try
            {
                _context.Maquina.Remove(maquina);
                await _context.SaveChangesAsync();
                return Ok($"La máquina con ID {id} fue eliminada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("No se pudo eliminar la máquina porque está relacionada con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
