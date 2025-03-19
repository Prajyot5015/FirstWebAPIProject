using FirstWebAPIProject.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstWebAPIProject.Repository.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration) // Inject IConfiguration into the constructor to access appsettings.json
        {
            this.configuration = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // Create Claims


            var claims = new List<Claim>(); 

            claims.Add(new Claim(ClaimTypes.Email, user.Email)); //JWT tokens store user details as claims, and email is useful for identification.

            foreach (var role in roles)  
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // Add roles to the claims
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])); // Create a symmetric security key using the secret key from appsettings.json

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Create signing credentials using the key and the HMAC-SHA256 algorithm

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],      // The issuer (who created the token)
                configuration["Jwt:Audience"],    // The audience (who can use the token)
                claims,                           // The user’s claims (like username, roles, etc.)
                expires: DateTime.Now.AddMinutes(15),  // Token expiration time (valid for 15 mins)
                signingCredentials: credentials   // Signing credentials (to secure the token)
            );


            return new JwtSecurityTokenHandler().WriteToken(token); // convert token to string and return it

        }
    }
}
