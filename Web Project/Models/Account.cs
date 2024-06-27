using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Project.Models
{
    public class Account
    {
        public string ID { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string name { get; set; }

        [Required(ErrorMessage = "Required!")]
        public string surname { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Please enter a valid mail.")]
        public string mail { get; set; }

        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,12}$", ErrorMessage = "Please enter a valid password.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Required!")]
        [Compare("password", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string confirm_password { get; set; }

    }
}