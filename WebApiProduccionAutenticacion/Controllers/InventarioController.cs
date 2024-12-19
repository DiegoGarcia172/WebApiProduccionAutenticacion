using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApiProduccionAutenticacion.Data;
using WebApiProduccionAutenticacion.Models;

namespace WebApiProduccionAutenticacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventarioController : ControllerBase
    {
        private readonly ProduccionDbContext _context;

        public InventarioController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventario>>> GetInventario()
        {
            return await _context.Inventario.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Inventario>> GetInventariobyId(int id)
        {
            var Inventario = await _context.Inventario.FindAsync(id);

            if (Inventario == null)
            {
                return NotFound();

            }

            return Ok(Inventario);
        }
        [HttpPost]
        public async Task<ActionResult<Inventario>> CrearInventario([FromBody] Inventario inventario)
        {
            if (inventario == null)
            {
                return BadRequest();
            }
            inventario.ID = 0;
            _context.Inventario.Add(inventario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInventariobyId), new { id = inventario.ID }, inventario);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarInventario(int id)
        {
            var inventario = await _context.Inventario.FindAsync(id);

            if (inventario == null)
            {
                return NotFound($"El registro de inventario con ID {id} no existe.");
            }

            try
            {
                _context.Inventario.Remove(inventario);
                await _context.SaveChangesAsync();
                return Ok($"El registro de inventario con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo eliminar el registro de inventario porque está relacionado con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarInventario(int id, [FromBody] Inventario inventarioActualizado)
        {
            if (id != inventarioActualizado.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var inventarioExistente = await _context.Inventario.FindAsync(id);

            if (inventarioExistente == null)
            {
                return NotFound($"El registro de inventario con ID {id} no existe.");
            }
            inventarioExistente.id_producto = inventarioActualizado.id_producto;
            inventarioExistente.id_materiaprima = inventarioActualizado.id_materiaprima;
            inventarioExistente.id_almacen = inventarioActualizado.id_almacen;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"El registro de inventario con ID {id} fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el registro de inventario: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
