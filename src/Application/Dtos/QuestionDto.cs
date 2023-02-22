namespace Application.Dtos
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public bool IsMandatory { get; set; }

    }
}
