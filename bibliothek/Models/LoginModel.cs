using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace bibliothek.Models
{
    
    public class LoginModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Passwort { get; set; }
    }
}