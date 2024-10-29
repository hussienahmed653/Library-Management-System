using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library_Management_System.DTOs
{
    public class JwtTokenHealper
    {
        public static string GenerateToken(string type,int id, string secretkey)
        {
            Claim[] claims;
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
            var Credential = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            claims = new[]
            {
                new Claim(ClaimTypes.Role, type),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            };

            var token = new JwtSecurityToken
            (
                issuer: "Your Issuer",
                audience: "Your Audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Credential
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
