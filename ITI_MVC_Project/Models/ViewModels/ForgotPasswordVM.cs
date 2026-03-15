using System.ComponentModel.DataAnnotations;

namespace ITI_MVC_Project.Models.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
