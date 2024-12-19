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
    public class ProductoController : ControllerBase
    {
        private readonly ProduccionDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductoController(ProduccionDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Producto.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductoById(int id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto([FromBody] Producto producto)
        {
            if (producto == null)
            {
                return BadRequest();
            }

            producto.ID = 0;
            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductoById), new { id = producto.ID }, producto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto productoActualizado)
        {
            if (id != productoActualizado.ID)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID del objeto enviado.");
            }

            var productoExistente = await _context.Producto.FindAsync(id);

            if (productoExistente == null)
            {
                return NotFound($"El producto con ID {id} no existe.");
            }

            productoExistente.Nombre = productoActualizado.Nombre;
            productoExistente.Cantidad = productoActualizado.Cantidad;
            productoExistente.Calidad = productoActualizado.Calidad;
            productoExistente.Fecha_Fin = productoActualizado.Fecha_Fin;
            productoExistente.Descripcion = productoActualizado.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
                return Ok($"El producto con ID {id} fue actualizado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el producto: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound($"El producto con ID {id} no existe.");
            }

            try
            {
                _context.Producto.Remove(producto);
                await _context.SaveChangesAsync();
                return Ok($"El producto con ID {id} fue eliminado exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("No se pudo eliminar el producto porque está relacionado con otros datos. Elimina esos datos primero.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }
    }
}
