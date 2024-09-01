using Alianza.Models;
using Alianza.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alianza.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AlianzaContext _context;
        private Exception? exception = null;

        public AuthController ( AlianzaContext context , TokenService tokenService )
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost ( "Login" )]
        public async Task<IActionResult> LoginAsync ( [FromBody] LoginRequest loginRequest )
        {
            try
            {
                // Buscar el usuario directamente en la base de datos en lugar de cargar todos los usuarios
                var encryptedPassword = EncryptPasswords.EncryptPassword ( loginRequest.Password );
                var user = await _context.Users
                    .FirstOrDefaultAsync ( user => user.Email == loginRequest.Email && user.Password == encryptedPassword );

                if (user != null)
                {
                    var token = _tokenService.GenerateToken ( loginRequest.Email );

                    // Crear el objeto ResponseData para la respuesta exitosa
                    var response = new ResponseData
                    {
                        Data = new { Token = token } ,
                        Message = "Autenticación exitosa" ,
                        Status = 200 // Código de estado HTTP 200 (OK)
                    };

                    return Ok ( response );
                }

                // Respuesta en caso de credenciales incorrectas
                return Unauthorized ( new ResponseData
                {
                    Data = null ,
                    Message = "Credenciales incorrectas" ,
                    Status = 401 // Código de estado HTTP 401 (Unauthorized)
                } );
            }
            catch (Exception ex)
            {
                // Manejo de la excepción y devolución de un mensaje de error apropiado
                return BadRequest ( new ResponseData
                {
                    Data = null ,
                    Message = $"Error al intentar iniciar sesión: {ex.Message}" ,
                    Status = 400 // Código de estado HTTP 400 (Bad Request)
                } );
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
