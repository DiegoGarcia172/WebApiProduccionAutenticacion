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
    public class AlmacenController : Controller
    {
        private readonly ProduccionDbContext _context;
        public AlmacenController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Almacen>>> GetAlmacen()
        {
            return await _context.Almacen.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Almacen>> GetAlmacenById(int id)
        {
            var almacen = await _context.Almacen.FindAsync(id);

            if (almacen == null)
            {
                return NotFound();

            }

            return Ok(almacen);
        }
        [HttpPost]
        public async Task<ActionResult<Almacen>> CrearAlmacen([FromBody] Almacen almacen)
        {
            if (almacen == null)
            {
                return BadRequest();
            }
            almacen.ID = 0;
            _context.Almacen.Add(almacen);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAlmacenById), new { id = almacen.ID }, almacen);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAlmacen(int id)
        {
            var almacen = await _context.Almacen.FindAsync(id);

            if (almacen == null)
            {
                return NotFound($"El registro de almacén con ID {id} no existe.");
            }

            try
            {
                _context.Almacen.Remove(almacen);
                await _context.SaveChangesAsync();
                return Ok($"El registro de almacén con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo eliminar el registro de almacén porque está relacionado con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarAlmacen(int id, [FromBody] Almacen almacenActualizado)
        {
            if (id != almacenActualizado.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var almacenExistente = await _context.Almacen.FindAsync(id);

            if (almacenExistente == null)
            {
                return NotFound($"El registro de almacén con ID {id} no existe.");
            }
            almacenExistente.Nombre = almacenActualizado.Nombre;
            almacenExistente.id_producto = almacenActualizado.id_producto;
            almacenExistente.id_orden = almacenActualizado.id_orden;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"El registro de almacén con ID {id} fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el registro de almacén: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
