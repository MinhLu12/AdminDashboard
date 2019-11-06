using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator;
using AdminDashboard.Exceptions;
using AdminDashboard.Main.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GravitationalTest.BusinessOrchestrators.Users
{
    public class LoginOrchestrator : ILoginOrchestrator
    {
        private readonly string Username = "Minh";
        private static readonly int SALT_BYTE_SIZE = 16; // byte array size for the salt
        private static readonly int HASH_BYTE_SIZE = 20; // byte array size for the hash, together with salt is 36 bytes

        private readonly AuthorizationConfiguration Configuration;

        public LoginOrchestrator(IOptions<AuthorizationConfiguration> configuration)
        {
            Configuration = configuration.Value;
        }

        public string Authenticate(string username, string password)
        {
            if (UsernameIsIncorrect(username))
                throw new InvalidCredentialsException();

            if (IsPasswordIncorrect(password))
                throw new InvalidCredentialsException();

            return GenerateJwtToken();
        }

        private bool IsPasswordIncorrect(string password)
        {
            string savedPasswordHash = Configuration.Secret;
            byte[] expectedHash = GetBytesFrom(savedPasswordHash);
            byte[] salt = new byte[SALT_BYTE_SIZE];
            Array.Copy(expectedHash, 0, salt, 0, SALT_BYTE_SIZE);

            byte[] actualHash = HashPasswordAttempt(password, salt);

            for (int i = 0; i < HASH_BYTE_SIZE; i++)
                if (expectedHash[i + SALT_BYTE_SIZE] != actualHash[i])
                    return true;

            return false;
        }

        private static byte[] HashPasswordAttempt(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(HASH_BYTE_SIZE);
            return hash;
        }

        private static byte[] GetBytesFrom(string savedPasswordHash)
        {
            return Convert.FromBase64String(savedPasswordHash);
        }

        private bool UsernameIsIncorrect(string username)
        {
            return Username != username;
        }

        private string GenerateJwtToken()
        {
            byte[] key = GetSigningKey();

            SecurityTokenDescriptor details = GenerateTokenDetailsWith(key);

            return WriteTokenWith(details);
        }

        private static string WriteTokenWith(SecurityTokenDescriptor details)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(details);
            return tokenHandler.WriteToken(token);
        }

        private byte[] GetSigningKey()
        {
            return Encoding.ASCII.GetBytes(Configuration.Bearer);
        }

        private static SecurityTokenDescriptor GenerateTokenDetailsWith(byte[] key)
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
