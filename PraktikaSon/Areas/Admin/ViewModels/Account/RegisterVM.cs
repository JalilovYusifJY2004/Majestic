﻿using System.ComponentModel.DataAnnotations;

namespace PraktikaSon.Areas.Admin.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Max 25 char")]
        public string Name { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Max 25 char")]
        public string Surname { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Max 25 char")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Max 25 char")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
