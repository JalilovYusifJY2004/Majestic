﻿using System.ComponentModel.DataAnnotations;

namespace PraktikaSon2.Areas.Admin.ViewModels.Account
{
	public class RegisterVM
	{
        [Required]
        [MaxLength(25,ErrorMessage ="max 25 ")]
        public string Name { get; set; }
        [Required]

        public string Surname { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set;}
        [Required]
        [DataType(DataType.Password)]
        public  string Password { get; set;}
        [Required]
		[DataType(DataType.Password)]
        [Compare(nameof(Password))]
		public string ConfirmPassword { get; set;}
    }
}
