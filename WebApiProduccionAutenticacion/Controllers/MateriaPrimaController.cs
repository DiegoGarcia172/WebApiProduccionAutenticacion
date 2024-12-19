using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProduccionAutenticacion.Data;
using WebApiProduccionAutenticacion.Models;

namespace WebApiProduccionAutenticacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MateriaPrimaController : ControllerBase
    {
        private readonly ProduccionDbContext _context;

        public MateriaPrimaController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaPrima>>> GetMateriaPrima()
        {
            return await _context.MateriaPrima.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaPrima>> GetMateriaPrimaById(int id)
        {
            var MateriaPrima = await _context.MateriaPrima.FindAsync(id);

            if (MateriaPrima == null)
            {
                return NotFound();

            }

            return Ok(MateriaPrima);
        }
        [HttpPost]
        public async Task<ActionResult<MateriaPrima>> CrearMateriaPrima([FromBody] MateriaPrima materiprima)
        {
            if (materiprima == null)
            {
                return BadRequest();
            }
            materiprima.ID = 0;
            _context.MateriaPrima.Add(materiprima);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMateriaPrimaById), new { id = materiprima.ID }, materiprima);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMateriaPrima(int id)
        {
            var materiaPrima = await _context.MateriaPrima.FindAsync(id);

            if (materiaPrima == null)
            {
                return NotFound($"La materia prima con ID {id} no existe.");
            }

            try
            {
                _context.MateriaPrima.Remove(materiaPrima);
                await _context.SaveChangesAsync();
                return Ok($"La materia prima con ID {id} fue eliminada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("No se pudo eliminar la materia prima porque está relacionada con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMateriaPrima(int id, [FromBody] MateriaPrima materiaActualizada)
        {
            if (id != materiaActualizada.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var materiaExistente = await _context.MateriaPrima.FindAsync(id);

            if (materiaExistente == null)
            {
                return NotFound($"La materia prima con ID {id} no existe.");
            }
            materiaExistente.ControlUnitario = materiaActualizada.ControlUnitario;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"La materia prima con ID {id} fue actualizada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar la materia prima: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
