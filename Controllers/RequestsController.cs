using Alianza.Models;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alianza.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly AlianzaContext _context;

        public RequestsController ( AlianzaContext context )
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Request>>> GetRequests ( )
        {
            return await _context.Requests.ToListAsync ( );
        }

        // GET: api/Requests/5
        [HttpGet ( "{id}" )]
        public async Task<ActionResult<Models.Request>> GetRequest ( int id )
        {
            var request = await _context.Requests.FindAsync ( id );

            if (request == null)
            {
                return NotFound ( );
            }

            return request;
        }

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<Models.Request>> PostRequest ( Models.Request request )
        {
            _context.Requests.Add ( request );
            await _context.SaveChangesAsync ( );

            return CreatedAtAction ( nameof ( GetRequest ) , new { id = request.Id } , request );
        }

        // PUT: api/Requests/5
        [HttpPut ( "{id}" )]
        public async Task<IActionResult> PutRequest ( int id , Models.Request request )
        {
            if (id != request.Id)
            {
                return BadRequest ( );
            }

            _context.Entry ( request ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync ( );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists ( id ))
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

        // DELETE: api/Requests/5
        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteRequest ( int id )
        {
            var request = await _context.Requests.FindAsync ( id );
            if (request == null)
            {
                return NotFound ( );
            }

            _context.Requests.Remove ( request );
            await _context.SaveChangesAsync ( );

            return NoContent ( );
        }

        private bool RequestExists ( int id )
        {
            return _context.Requests.Any ( e => e.Id == id );
        }
    }

}
