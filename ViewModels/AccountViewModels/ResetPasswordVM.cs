﻿using System.ComponentModel.DataAnnotations;

namespace Wolmart.Ecommerce.ViewModels.AccountViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool IsSuccess { get; set; }
    }
}
