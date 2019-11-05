using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator;
using AdminDashboard.BusinessLogicOrchestrators.LoginOrchestrator.PasswordHasher;
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
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly string Username = "test";
        private readonly string Password = "GravitationalInterviewByMinhNovember2019";

        private readonly AuthorizationConfiguration Configuration;

        public LoginOrchestrator(IOptions<AuthorizationConfiguration> configuration)
        {
            Configuration = configuration.Value;
        }

        public string Authenticate(string username, string password)
        {
            var salt = Salt.Create();
            var hash = Hash.Create(password, salt); 
            bool match = Hash.Validate(password, salt, hash); // this "password" should reach into dotnet secrets

            // How to store this password as a hash?
            // With the password that outputs, manually store that in dotnet secrets
            byte[] salts;
            new RNGCryptoServiceProvider().GetBytes(salts = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(Password, salts, 10000);
            byte[] hashs = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salts, 0, hashBytes, 0, 16);
            Array.Copy(hashs, 0, hashBytes, 16, 20);

            //OUTPUT
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            // COMPARISON LOGIC. Does Password have to be hashed first?
            // Now, retrieve this.
            // RY25waYFu+VlSdPUikfbLJEUpt2SuD5rF2bkQMAqwJ+N+6hm
            Compare("N/s5+iWPYn3I0ZS4O10JU4+zIr/vCFTBoEK7bSc5/utA4cPW");

            // TODO: Delete
            // This is to hash the password. How?
            if (Username != username || Password != password)
                throw new InvalidCredentialsException();

            // authentication successful so generate jwt token
            return GenerateJwtToken();
        }

        private void Compare(string savedPasswordHash)
        {
            /* Fetch the stored value */
            //string savedPasswordHash = DBContext.GetUser(u => u.UserName == user).Password;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    throw new UnauthorizedAccessException();
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
            return Encoding.ASCII.GetBytes(Configuration.Bearer);
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
