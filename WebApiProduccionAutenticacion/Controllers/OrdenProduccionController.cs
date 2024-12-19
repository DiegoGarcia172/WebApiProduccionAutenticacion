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
    public class OrdenProduccionController : Controller
    {
        private readonly ProduccionDbContext _context;

        public OrdenProduccionController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenProduccion>>> GetOrdenProduccion()
        {
            return await _context.OrdenProduccion.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenProduccion>> GetOrdenProduccionById(int id)
        {
            var OrdenProduccion = await _context.OrdenProduccion.FindAsync(id);

            if (OrdenProduccion == null)
            {
                return NotFound();

            }

            return Ok(OrdenProduccion);
        }
        [HttpPost]
        public async Task<ActionResult<OrdenProduccion>> CrearOrdenProduccion([FromBody] OrdenProduccion ordenProduccion)
        {
            if (ordenProduccion == null)
            {
                return BadRequest();
            }
            ordenProduccion.ID = 0;
            _context.OrdenProduccion.Add(ordenProduccion);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrdenProduccionById), new { id = ordenProduccion.ID }, ordenProduccion);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarOrdenProduccion(int id, [FromBody] OrdenProduccion ordenActualizada)
        {
            if (id != ordenActualizada.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var ordenExistente = await _context.OrdenProduccion.FindAsync(id);

            if (ordenExistente == null)
            {
                return NotFound($"La orden de producción con ID {id} no existe.");
            }
            ordenExistente.Fecha_Orden = ordenActualizada.Fecha_Orden;
            ordenExistente.Cantidad = ordenActualizada.Cantidad;
            ordenExistente.Fecha_Entrega = ordenActualizada.Fecha_Entrega;
            ordenExistente.id_empleado = ordenActualizada.id_empleado;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"La orden de producción con ID {id} fue actualizada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar la orden de producción: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarOrdenProduccion(int id)
        {
            var orden = await _context.OrdenProduccion.FindAsync(id);

            if (orden == null)
            {
                return NotFound($"La orden de producción con ID {id} no existe.");
            }

            try
            {
                _context.OrdenProduccion.Remove(orden);
                await _context.SaveChangesAsync();
                return Ok($"La orden de producción con ID {id} fue eliminada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("No se pudo eliminar la orden de producción porque está relacionada con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
