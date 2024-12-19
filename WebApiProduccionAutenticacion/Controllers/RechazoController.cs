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
    public class RechazoController : ControllerBase
    {
        private readonly ProduccionDbContext _context;

        public RechazoController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rechazo>>> GetRechazo()
        {
            return await _context.Rechazo.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Proceso>> GetRechazoById(int id)
        {
            var Rechazo = await _context.Rechazo.FindAsync(id);

            if (Rechazo == null)
            {
                return NotFound();

            }

            return Ok(Rechazo);
        }
        [HttpPost]
        public async Task<ActionResult<Rechazo>> CrearRechazo([FromBody] Rechazo rechazo)
        {
            if (rechazo == null)
            {
                return BadRequest();
            }
            rechazo.ID = 0;
            _context.Rechazo.Add(rechazo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRechazoById), new { id = rechazo.ID }, rechazo);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarRechazo(int id, [FromBody] Rechazo rechazoActualizado)
        {
            if (id != rechazoActualizado.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }
            var rechazoExistente = await _context.Rechazo.FindAsync(id);
            if (rechazoExistente == null)
            {
                return NotFound($"El rechazo con ID {id} no existe.");
            }
            rechazoExistente.id_producto = rechazoActualizado.id_producto;
            rechazoExistente.CantidadPR = rechazoActualizado.CantidadPR;
            rechazoExistente.Fecha_Hora = rechazoActualizado.Fecha_Hora;
            rechazoExistente.Descripcion = rechazoActualizado.Descripcion;
            try
            {
                await _context.SaveChangesAsync();
                return Ok($"El rechazo con ID {id} fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el rechazo: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRechazo(int id)
        {
            try
            {
                var rechazo = await _context.Rechazo.FindAsync(id);

                if (rechazo == null)
                {
                    return NotFound($"El rechazo con ID {id} no existe.");
                }
                _context.Rechazo.Remove(rechazo);
                await _context.SaveChangesAsync();
                return Ok($"El rechazo con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return BadRequest("No se pudo borrar porque hay más datos registrados asociados a este rechazo. Por favor, elimine primero los datos relacionados.");
                }
                return StatusCode(500, "Ocurrió un error al intentar eliminar el rechazo.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
