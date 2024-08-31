using Alianza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alianza.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AccountingAccountsController : ControllerBase
    {
        private readonly AlianzaContext _context;

        public AccountingAccountsController ( AlianzaContext context )
        {
            _context = context;
        }

        // GET: api/AccountingAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountingAccount>>> GetAccountingAccounts ( )
        {
            return await _context.AccountingAccounts.ToListAsync ( );
        }

        // GET: api/AccountingAccounts/5
        [HttpGet ( "{id}" )]
        public async Task<ActionResult<AccountingAccount>> GetAccountingAccount ( int id )
        {
            var accountingAccount = await _context.AccountingAccounts.FindAsync ( id );

            if (accountingAccount == null)
            {
                return NotFound ( );
            }

            return accountingAccount;
        }

        // POST: api/AccountingAccounts
        [HttpPost]
        public async Task<ActionResult<AccountingAccount>> PostAccountingAccount ( AccountingAccount accountingAccount )
        {
            _context.AccountingAccounts.Add ( accountingAccount );
            await _context.SaveChangesAsync ( );

            return CreatedAtAction ( nameof ( GetAccountingAccount ) , new { id = accountingAccount.Id } , accountingAccount );
        }

        // PUT: api/AccountingAccounts/5
        [HttpPut ( "{id}" )]
        public async Task<IActionResult> PutAccountingAccount ( int id , AccountingAccount accountingAccount )
        {
            if (id != accountingAccount.Id)
            {
                return BadRequest ( );
            }

            _context.Entry ( accountingAccount ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync ( );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountingAccountExists ( id ))
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

        // DELETE: api/AccountingAccounts/5
        [HttpDelete ( "{id}" )]
        public async Task<IActionResult> DeleteAccountingAccount ( int id )
        {
            var accountingAccount = await _context.AccountingAccounts.FindAsync ( id );
            if (accountingAccount == null)
            {
                return NotFound ( );
            }

            _context.AccountingAccounts.Remove ( accountingAccount );
            await _context.SaveChangesAsync ( );

            return NoContent ( );
        }

        private bool AccountingAccountExists ( int id )
        {
            return _context.AccountingAccounts.Any ( e => e.Id == id );
        }
    }
}
