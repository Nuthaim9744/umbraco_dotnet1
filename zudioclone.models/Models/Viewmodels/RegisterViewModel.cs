using System.ComponentModel.DataAnnotations;

namespace zudioclone.models.Models.Viewmodels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "not exceed more than 50 characters")]
        public string Name { get; set; }



        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com)$", ErrorMessage = "pls enter valid email address ending with '.com'.")]
        [EmailAddress(ErrorMessage = "invalid email id")]

        public string Email { get; set; }

        [Required(ErrorMessage = "pls enter valid password")]
        [MinLength(10, ErrorMessage = "password must contain atleast 10 characters")]
        public string Password { get; set; }

    }



}
