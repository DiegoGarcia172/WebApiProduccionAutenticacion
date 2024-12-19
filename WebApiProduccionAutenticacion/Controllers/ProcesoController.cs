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
    public class ProcesoController : ControllerBase
    {
        private readonly ProduccionDbContext _context;

        public ProcesoController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proceso>>> GetProceso()
        {
            return await _context.Proceso.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proceso>> GetProcesoById(int id)
        {
            var proceso = await _context.Proceso.FindAsync(id);

            if (proceso == null)
            {
                return NotFound();

            }

            return Ok(proceso);
        }
        [HttpPost]
        public async Task<ActionResult<Proceso>> CrearProceso([FromBody] Proceso proceso)
        {
            if (proceso == null)
            {
                return BadRequest();
            }
            proceso.ID = 0;
            _context.Proceso.Add(proceso);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProcesoById), new { id = proceso.ID }, proceso);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProceso(int id, [FromBody] Proceso proceso)
        {
            if (id != proceso.ID)
            {
                return BadRequest("El ID del proceso no coincide con el ID de la URL.");
            }
            var procesoExistente = await _context.Proceso.FindAsync(id);
            if (procesoExistente == null)
            {
                return NotFound($"El proceso con ID {id} no existe.");
            }
            procesoExistente.Nombre = proceso.Nombre;
            procesoExistente.Descripcion = proceso.Descripcion;
            _context.Entry(procesoExistente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Proceso.Any(e => e.ID == id))
                {
                    return NotFound($"El proceso con ID {id} no se encontró durante la actualización.");
                }
                throw;
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProceso(int id)
        {
            try
            {
                var proceso = await _context.Proceso.FindAsync(id);

                if (proceso == null)
                {
                    return NotFound($"El proceso con ID {id} no existe.");
                }
                _context.Proceso.Remove(proceso);
                await _context.SaveChangesAsync();
                return Ok($"El proceso con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return BadRequest("No se pudo borrar porque hay más datos registrados asociados a este proceso. Por favor, elimine primero los datos relacionados.");
                }
                return StatusCode(500, "Ocurrió un error al intentar eliminar el proceso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
