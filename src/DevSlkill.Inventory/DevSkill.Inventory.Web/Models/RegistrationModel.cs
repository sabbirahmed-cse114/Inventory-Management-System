﻿using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Models
{
	public class RegistrationModel
	{
        [Required]
        [Display(Name = "Username")]
        [StringLength(30, ErrorMessage = "At least 6 characters and at max 30 characters", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public string? ReturnUrl { get; set; }

		public IList<AuthenticationScheme>? ExternalLogins { get; set; }
	}
}
