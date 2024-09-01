using Alianza.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Alianza.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    [Authorize] // Este controlador requiere autenticación
    public class UsersController : ControllerBase
    {
        private readonly AlianzaContext _context;
        private Exception? exception = null;

        public UsersController ( AlianzaContext context )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers ( )
        {
            try
            {
                return await _context.Users.ToListAsync ( );
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return BadRequest ( exception.Message );
        }

        // GET: api/Users/5
        [HttpGet ( "{id}" )]
        public async Task<ActionResult<User>> GetUser ( int id )
        {
            try
            {
                var user = await _context.Users.FindAsync ( id );

                if (user == null)
                {
                    return NotFound ( );
                }

                return user;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return BadRequest ( exception.Message );
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser ( User user )
        {
            try
            {
                var u = EncryptPassword ( user.Password );
                user.Password = u;

                _context.Users.Add ( user );
                await _context.SaveChangesAsync ( );

                return CreatedAtAction ( nameof ( GetUser ) , new { id = user.Id } , user );
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return BadRequest ( exception.Message );
        }

        // PUT: api/Users/5
        [HttpPut ( "{id}" )]
        public async Task<IActionResult> PutUser ( int id , User user )
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest ( );
                }

                _context.Entry ( user ).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync ( );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists ( id ))
                    {
                        return NotFound ( );
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent ( );
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return BadRequest ( exception.Message );
        }

        // DELETE: api/Users/5
        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteUser ( int id )
        {
            try
            {


                var user = await _context.Users.FindAsync ( id );
                if (user == null)
                {
                    return NotFound ( );
                }

                _context.Users.Remove ( user );
                await _context.SaveChangesAsync ( );

                return NoContent ( );
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return BadRequest ( exception.Message );
        }

        private bool UserExists ( int id )
        {
            try
            {
                return _context.Users.Any ( e => e.Id == id );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string EncryptPassword ( string password )
        {
            if (string.IsNullOrEmpty ( password ))
            {
                throw new ArgumentException ( "La contraseña no puede estar vacía." );
            }

            // Crear una instancia de SHA256
            using (SHA256 sha256 = SHA256.Create ( ))
            {
                // Convertir la contraseña en un array de bytes
                byte [] bytes = Encoding.UTF8.GetBytes ( password );

                // Calcular el hash de la contraseña
                byte [] hash = sha256.ComputeHash ( bytes );

                // Convertir el array de bytes en una cadena hexadecimal
                StringBuilder builder = new StringBuilder ( );
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append ( hash [ i ].ToString ( "x2" ) );
                }

                return builder.ToString ( ); // Devolver la contraseña encriptada
            }
        }
    }
}
