using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SSFR_MainAPI.Models
{
    public class UserSignUp
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }

    public class UserSignIn
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
