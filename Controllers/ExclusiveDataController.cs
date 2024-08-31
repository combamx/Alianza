using Alianza.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alianza.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class ExclusiveDataController : ControllerBase
    {
        private readonly AlianzaContext _context;

        public ExclusiveDataController ( AlianzaContext context )
        {
            _context = context;
        }

        // GET: api/ExclusiveData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExclusiveData>>> GetExclusiveData ( )
        {
            return await _context.ExclusiveData.ToListAsync ( );
        }

        // GET: api/ExclusiveData/5
        [HttpGet ( "{id}" )]
        public async Task<ActionResult<ExclusiveData>> GetExclusiveData ( int id )
        {
            var ExclusiveData = await _context.ExclusiveData.FindAsync ( id );

            if (ExclusiveData == null)
            {
                return NotFound ( );
            }

            return ExclusiveData;
        }

        // POST: api/ExclusiveData
        [HttpPost]
        public async Task<ActionResult<ExclusiveData>> PostExclusiveData ( ExclusiveData ExclusiveData )
        {
            _context.ExclusiveData.Add ( ExclusiveData );
            await _context.SaveChangesAsync ( );

            return CreatedAtAction ( nameof ( GetExclusiveData ) , new { id = ExclusiveData.Id } , ExclusiveData );
        }

        // PUT: api/ExclusiveData/5
        [HttpPut ( "{id}" )]
        public async Task<IActionResult> PutExclusiveData ( int id , ExclusiveData ExclusiveData )
        {
            if (id != ExclusiveData.Id)
            {
                return BadRequest ( );
            }

            _context.Entry ( ExclusiveData ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync ( );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExclusiveDataExists ( id ))
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

        // DELETE: api/ExclusiveData/5
        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteExclusiveData ( int id )
        {
            var ExclusiveData = await _context.ExclusiveData.FindAsync ( id );
            if (ExclusiveData == null)
            {
                return NotFound ( );
            }

            _context.ExclusiveData.Remove ( ExclusiveData );
            await _context.SaveChangesAsync ( );

            return NoContent ( );
        }

        private bool ExclusiveDataExists ( int id )
        {
            return _context.ExclusiveData.Any ( e => e.Id == id );
        }
    }
}
