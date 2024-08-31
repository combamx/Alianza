using Alianza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alianza.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AccountNumbersController : ControllerBase
    {
        private readonly AlianzaContext _context;

        public AccountNumbersController ( AlianzaContext context )
        {
            _context = context;
        }

        // GET: api/AccountNumbers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountNumber>>> GetAccountNumbers ( )
        {
            return await _context.AccountNumbers.ToListAsync ( );
        }

        // GET: api/AccountNumbers/5
        [HttpGet ( "{id}" )]
        public async Task<ActionResult<AccountNumber>> GetAccountNumber ( int id )
        {
            var accountNumber = await _context.AccountNumbers.FindAsync ( id );

            if (accountNumber == null)
            {
                return NotFound ( );
            }

            return accountNumber;
        }

        // POST: api/AccountNumbers
        [HttpPost]
        public async Task<ActionResult<AccountNumber>> PostAccountNumber ( AccountNumber accountNumber )
        {
            _context.AccountNumbers.Add ( accountNumber );
            await _context.SaveChangesAsync ( );

            return CreatedAtAction ( nameof ( GetAccountNumber ) , new { id = accountNumber.Id } , accountNumber );
        }

        // PUT: api/AccountNumbers/5
        [HttpPut ( "{id}" )]
        public async Task<IActionResult> PutAccountNumber ( int id , AccountNumber accountNumber )
        {
            if (id != accountNumber.Id)
            {
                return BadRequest ( );
            }

            _context.Entry ( accountNumber ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync ( );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountNumberExists ( id ))
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

        // DELETE: api/AccountNumbers/5
        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteAccountNumber ( int id )
        {
            var accountNumber = await _context.AccountNumbers.FindAsync ( id );
            if (accountNumber == null)
            {
                return NotFound ( );
            }

            _context.AccountNumbers.Remove ( accountNumber );
            await _context.SaveChangesAsync ( );

            return NoContent ( );
        }

        private bool AccountNumberExists ( int id )
        {
            return _context.AccountNumbers.Any ( e => e.Id == id );
        }
    }
}
