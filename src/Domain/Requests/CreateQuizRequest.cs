﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Requests
{
    public class CreateQuizRequest
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
    }
}
