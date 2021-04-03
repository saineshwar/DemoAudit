using System.ComponentModel.DataAnnotations;

namespace DemoAudit.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter UserId")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
    }
}