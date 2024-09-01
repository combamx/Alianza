using Alianza.Models;
using Alianza.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                var u = EncryptPasswords.EncryptPassword ( user.Password );
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


    }
}
