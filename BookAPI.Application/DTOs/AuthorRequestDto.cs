﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Application.DTOs
{
    public class AuthorRequestDto
    {
        [Required(ErrorMessage = "The FirstName field is required.")]
        [MinLength(3, ErrorMessage = "The FirstName must be at least 3 characters long.")]
        [MaxLength(15, ErrorMessage = "The FirstName cannot exceed 15 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required.")]
        [MinLength(3, ErrorMessage = "The LastName must be at least 3 characters long.")]
        [MaxLength(50, ErrorMessage = "The LastName cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The BirthYear field is required.")]
        public DateTime BirthYear { get; set; }
    }
}
