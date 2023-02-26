using System.ComponentModel.DataAnnotations;

namespace Domain.Requests
{
    public class UpdateQuestionOptionRequest
    {
        [Required]
        public string Text { get; set; }
        
        [Required]
        public bool IsAnswer { get; set; }
    }
}
