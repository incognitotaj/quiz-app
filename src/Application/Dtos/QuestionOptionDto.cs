using Domain.Entities;

namespace Application.Dtos
{
    public class QuestionOptionDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
    }
}
