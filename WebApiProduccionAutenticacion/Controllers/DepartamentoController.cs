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
    public class DepartamentoController : Controller
    {
        private readonly ProduccionDbContext _context;

        public DepartamentoController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamento()
        {
            return await _context.Departamento.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamentoById(int id)
        {
            var departamento = await _context.Departamento.FindAsync(id);

            if (departamento == null)
            {
                return NotFound();

            }

            return Ok(departamento);
        }
        [HttpPost]
        public async Task<ActionResult<Departamento>> CrearDepatamento([FromBody] Departamento departamento)
        {
            if (departamento == null)
            {
                return BadRequest();
            }
            departamento.ID = 0;
            _context.Departamento.Add(departamento);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDepartamentoById), new { id = departamento.ID }, departamento);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarDepartamento(int id)
        {
            var departamento = await _context.Departamento.FindAsync(id);

            if (departamento == null)
            {
                return NotFound($"El registro de departamento con ID {id} no existe.");
            }

            try
            {
                _context.Departamento.Remove(departamento);
                await _context.SaveChangesAsync();
                return Ok($"El registro de departamento con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo eliminar el registro de departamento porque está relacionado con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarDepartamento(int id, [FromBody] Departamento departamentoActualizado)
        {
            if (id != departamentoActualizado.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var departamentoExistente = await _context.Departamento.FindAsync(id);

            if (departamentoExistente == null)
            {
                return NotFound($"El registro de departamento con ID {id} no existe.");
            }
            departamentoExistente.Nombre = departamentoActualizado.Nombre;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"El registro de departamento con ID {id} fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el registro de departamento: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
