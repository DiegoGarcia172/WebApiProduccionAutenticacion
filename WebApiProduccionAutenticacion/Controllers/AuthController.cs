using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProduccionAutenticacion.Custom;
using WebApiProduccionAutenticacion.Data;
using WebApiProduccionAutenticacion.Models;
using WebApiProduccionAutenticacion.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace WebApiProduccionAutenticacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly ProduccionDbContext _dbContext;
        private readonly Utilidades _utilidades;
        private readonly IConfiguration _configuration;

        public AccesoController(ProduccionDbContext dbContext, Utilidades utilidades, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _utilidades = utilidades;
            _configuration = configuration;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar(UsuarioDTO objeto)
        {
            // Verificar si el correo ya existe
            if (await _dbContext.Usuario.AnyAsync(u => u.email == objeto.Correo))
            {
                return BadRequest(new { IsSuccess = false, message = "El correo ya está registrado" });
            }

            // Crear el usuario
            var usuario = new Usuario
            {
                username = objeto.Nombre,
                email = objeto.Correo,
                password = _utilidades.encriptar(objeto.Clave)
            };

            try
            {
                await _dbContext.Usuario.AddAsync(usuario);
                await _dbContext.SaveChangesAsync();
                return Ok(new { IsSuccess = true, message = "Usuario registrado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { IsSuccess = false, message = $"Error al registrar usuario: {ex.Message}" });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsuarioDTO objeto)
        {
            // Validar usuario
            var usuario = await _dbContext.Usuario
                .FirstOrDefaultAsync(u =>
                    u.email == objeto.Correo &&
                    u.password == _utilidades.encriptar(objeto.Clave));

            if (usuario == null)
            {
                return Unauthorized(new { IsSuccess = false, message = "Usuario o clave incorrecta" });
            }

            // Generar token JWT
            var token = _utilidades.generarJWT(usuario);

            return Ok(new
            {
                IsSuccess = true,
                token,
                message = "Inicio de sesión exitoso"
            });
        }
    }
}
