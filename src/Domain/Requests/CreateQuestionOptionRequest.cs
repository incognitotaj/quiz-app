using System.ComponentModel.DataAnnotations;

namespace Domain.Requests
{
    public class CreateQuestionOptionRequest
    {
        [Required]
        public string Text { get; set; }
        
        [Required]
        public bool IsAnswer { get; set; }
    }
}
