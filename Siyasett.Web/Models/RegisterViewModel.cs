using System.ComponentModel.DataAnnotations;

namespace Siyasett.Web.Models
{
    public class RegisterViewModel
    {

        [Required(AllowEmptyStrings =false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]

        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [System.ComponentModel.DataAnnotations.Compare(otherProperty: "PasswordConfirm")]
        [MinLength(6)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MinLength(6)]

        public string PasswordConfirm { get; set; }


    }
}
