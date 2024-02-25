﻿using Azure.Identity;
using System.ComponentModel.DataAnnotations;

namespace NZWalk.API.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string[] Roles { get; set; }
    }
}
