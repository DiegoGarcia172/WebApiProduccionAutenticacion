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
    public class EmpleadoController : Controller
    {
        private readonly ProduccionDbContext _context;

        public EmpleadoController(ProduccionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleado()
        {
            return await _context.Empleado.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleadoById(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();

            }

            return Ok(empleado);
        }
        [HttpPost]
        public async Task<ActionResult<Empleado>> CrearEmpleado([FromBody] Empleado empleado)
        {
            if (empleado == null)
            {
                return BadRequest();
            }
            empleado.ID = 0;
            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmpleadoById), new { id = empleado.ID }, empleado);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);

            if (empleado == null)
            {
                return NotFound($"El registro de empleado con ID {id} no existe.");
            }

            try
            {
                _context.Empleado.Remove(empleado);
                await _context.SaveChangesAsync();
                return Ok($"El registro de empleado con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("No se pudo eliminar el registro de empleado porque está relacionado con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEmpleado(int id, [FromBody] Empleado empleadoActualizado)
        {
            if (id != empleadoActualizado.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var empleadoExistente = await _context.Empleado.FindAsync(id);

            if (empleadoExistente == null)
            {
                return NotFound($"El registro de empleado con ID {id} no existe.");
            }
            empleadoExistente.Nombre = empleadoActualizado.Nombre;
            empleadoExistente.Apellido_Paterno = empleadoActualizado.Apellido_Paterno;
            empleadoExistente.Apellido_Materno = empleadoActualizado.Apellido_Materno;
            empleadoExistente.Puesto = empleadoActualizado.Puesto;
            empleadoExistente.id_departamento = empleadoActualizado.id_departamento;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"El registro de empleado con ID {id} fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el registro de empleado: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

    }
}
