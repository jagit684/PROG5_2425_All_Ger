using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Wk8_JWT_API.Controllers
{
    /// <summary>
    /// Handles authentication operations such as login and JWT token generation.
    /// </summary>
    /// <remarks>
    /// Use this controller to authenticate users and retrieve a JWT token for accessing protected endpoints.
    /// </remarks>
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="login">The login request containing username and password.</param>
        /// <returns>
        /// A 200 OK response with a JWT token if authentication succeeds.
        /// </returns>
        /// <response code="200">Returns a JWT token in JSON format.</response>
        /// <response code="401">Unauthorized - invalid username or password.</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login.Username == "test" && login.Password == "password")
            {
                var token = GenerateJwtToken();
                return Ok(new { token });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Generates a JWT token with predefined claims.
        /// </summary>
        /// <returns>A signed JWT token string.</returns>
        private string GenerateJwtToken()
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is missing in configuration.");
            }

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var claims = new[]
            {
                new Claim("ictcommunityroom", "B107"),
                new Claim(JwtRegisteredClaimNames.Sub, "testuser"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Autorijbewijs", "Ger")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    /// <summary>
    /// Represents the login request payload.
    /// </summary>
    public class LoginModel
    {
        /// <summary>The username of the user.</summary>
        public required string Username { get; set; }

        /// <summary>The password of the user.</summary>
        public required string Password { get; set; }
    }
}