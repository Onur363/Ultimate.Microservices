using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.WebCore.Models
{
    public class SignInDto
    {
        [Required]
        [Display(Name ="Email adresi")]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Şifreniz")]
        public string Password { get; set; }
        
        [Display(Name ="Beni hatırla")]
        public bool IsRemember { get; set; }
    }
}
