using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alianza.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService ( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public string GenerateToken ( string email )
        {
            try
            {
                var jwtSettings = _configuration.GetSection ( "JwtSettings" );
                var secretKey = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes ( jwtSettings [ "SecretKey" ] ) );
                var credentials = new SigningCredentials ( secretKey , SecurityAlgorithms.HmacSha256 );

                var claims = new []
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken (
                    issuer: jwtSettings [ "Issuer" ] ,
                    audience: jwtSettings [ "Audience" ] ,
                    claims: claims ,
                    expires: DateTime.Now.AddMinutes ( Convert.ToDouble ( jwtSettings [ "ExpirationInMinutes" ] ) ) ,
                    signingCredentials: credentials );

                return new JwtSecurityTokenHandler ( ).WriteToken ( token );
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
