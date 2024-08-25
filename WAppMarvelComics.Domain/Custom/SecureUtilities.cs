using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Custom.Models;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Custom
{
    public class SecureUtilities(IOptions<SettingModel> setting) : ISecureUtilities
    {
        public string EncryptSHA256(string text)
        {
            // Computar el hash
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));

            // Convertir el array de bytes a string
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public string GenerateJWT(User user)
        {
            //crear la información del usuario para token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.Value.JwtKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }

        public async Task<bool> ValidateJWT(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(setting.Value.JwtKey!))
            };
            try
            {
                var validatedToken = await tokenHandler.ValidateTokenAsync(token, validationParameters);
                return validatedToken != null && validatedToken.IsValid;
            }
            catch
            {
                return false;
            }
        }
    }
}