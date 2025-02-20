
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT_Demo.HelperMethods
{
    public class Jwthelper
    {
        private readonly IConfiguration _configuration;
        private static HashSet<string> _revokedTokens = new HashSet<string>();
        private string secretKey;

        public Jwthelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId)
        {
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]))
                throw new InvalidOperationException("JWT secret key is missing in configuration.");

            // define security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // generating signature
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            // creating JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            // converting JWT object into string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       
        // extracting the token
        public string GetUserIdFromToken(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            return ValidateJwtToken(token);
        }

        public string ValidateJwtToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                return userIdClaim?.Value;
            }
            catch
            {
                return null;
            }
        }

        public void RevokeToken(string token)
        {
            _revokedTokens.Add(token);
        }
        public bool IsTokenRevoked(string token)
        {
            return _revokedTokens.Contains(token);
        }

    }
}
