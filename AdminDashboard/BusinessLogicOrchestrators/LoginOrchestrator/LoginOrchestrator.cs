using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator;
using AdminDashboard.Exceptions;
using AdminDashboard.Main.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GravitationalTest.BusinessOrchestrators.Users
{
    public class LoginOrchestrator : ILoginOrchestrator
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly string Username = "test";
        private readonly string Password = "test";

        private readonly AuthorizationConfiguration Configuration;

        public LoginOrchestrator(IOptions<AuthorizationConfiguration> configuration)
        {
            Configuration = configuration.Value;
        }

        public string Authenticate(string username, string password)
        {
            // TODO: Delete
            // This is to hash and compare!
            if (Username != username || Password != password)
                throw new InvalidCredentialsException();

            // authentication successful so generate jwt token
            return GenerateJwtToken();
        }

        private string GenerateJwtToken()
        {
            byte[] key = GetTokenKey();

            SecurityTokenDescriptor details = GenerateTokenDetailsFrom(key);

            return WriteTokenWith(details);
        }

        private static string WriteTokenWith(SecurityTokenDescriptor details)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(details);
            return tokenHandler.WriteToken(token);
        }

        private byte[] GetTokenKey()
        {
            return Encoding.ASCII.GetBytes(Configuration.Secret);
        }

        private static SecurityTokenDescriptor GenerateTokenDetailsFrom(byte[] key)
        {
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
