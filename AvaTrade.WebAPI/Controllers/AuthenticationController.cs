using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AvaTrade.WebAPI.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AvaTrade.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/auth/register")]
    public class AuthenticationController : ControllerBase
    {        
        private readonly AuthenticationIssuerOptions _authOptions;

        public AuthenticationController(IOptions<AuthenticationIssuerOptions> options)
        {
            _authOptions = options.Value;
        }

        [HttpPost]
        public IActionResult Register()
        {
            //Get the client id from the request header
            var clientId = Request.Headers["X-ClientId"].ToString();

            // Generate a new API key and secret for the client
            var apiKey = GenerateApiKey();
            var apiSecret = GenerateApiSecret();

            // Create a JWT token with the API key as the payload
            var token = GenerateJwtToken(apiKey);

            // TODO: Save the client ID, API key, and secret to a database or other storage mechanism

            // Return the JWT token and API key/secret to the client
            return Ok(new { Token = token, ApiKey = apiKey, ApiSecret = apiSecret });
        }

        private string GenerateApiKey()
        {
            // Generate a unique API key
            return Guid.NewGuid().ToString("N");
        }

        private string GenerateApiSecret()
        {
            // Generate a unique and secure API secret using a random number generator
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string GenerateJwtToken(string apiKey)
        {
            // Create a JWT token with the API key as the payload
            var key = Encoding.ASCII.GetBytes(_authOptions.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("apiKey", apiKey) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }        
    }
}
