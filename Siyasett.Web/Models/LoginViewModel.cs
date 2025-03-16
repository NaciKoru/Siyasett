using System.ComponentModel.DataAnnotations;

namespace Siyasett.Web.Models
{
    public class LoginViewModel
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Eposta { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string? ReturnUrl { get; set; }

        public bool BeniHatirla { get; set; }

    }
}
