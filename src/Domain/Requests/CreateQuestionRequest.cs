namespace Domain.Requests
{
    public class CreateQuestionRequest
    {
        public string Text { get; set; }
        public bool IsMandatory { get; set; }
    }
}
