using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models.JsonRequests
{
    public class AuthenticationRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
