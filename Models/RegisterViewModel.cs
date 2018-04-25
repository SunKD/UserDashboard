using System.ComponentModel.DataAnnotations;
using System;

namespace Dashboard.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name should be more than 2 characters")]
        [MinLength(2)]
        public string Name { get; set; }

        [Display(Name = "Alias")]
        [Required(ErrorMessage = "Alias should be more than 2 characters")]
        [MinLength(2)]
        public string NickName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password should be more than 8 characters")]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]
        public string ConfirmedPW { get; set; }

    }
}