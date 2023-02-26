using System.ComponentModel.DataAnnotations;
namespace Domain.Requests
{
    public class UpdateQuestionRequest
    {
        [Required]
        public string Text { get; set; }
        
        [Required]
        public bool IsMandatory { get; set; }
    }
}
