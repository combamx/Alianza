using Alianza.Models;
using Alianza.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // POST: api/Auth/Login
        [HttpPost ( "Login" )]
        public IActionResult Login ( [FromBody] LoginRequest loginRequest )
        {
            // Aquí deberías implementar la lógica de autenticación para verificar las credenciales
            // Por simplicidad, vamos a asumir que son válidas
            try
            {
                if (loginRequest.Email == "omar.cortes.casillas@gmail.com" && loginRequest.Password == "Admin2024")
                {
                    var token = _tokenService.GenerateToken ( loginRequest.Email );
                    return Ok ( new { Token = token } );
                }

                return Unauthorized ( );
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return BadRequest ( exception.Message );

        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
