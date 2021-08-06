using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests
{
    public class UserAddRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string Password { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Required]
        public string PasswordConfirm { get; set; }

        [StringLength(256, MinimumLength = 1)]
        [Required]
        public string AvatarUrl { get; set; }

        [StringLength(256, MinimumLength = 1)]
        [Required]
        public string TenantId { get; set; }
    }
}
