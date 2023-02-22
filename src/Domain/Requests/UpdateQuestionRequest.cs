namespace Domain.Requests
{
    public class UpdateQuestionRequest
    {
        public string Text { get; set; }
        public bool IsMandatory { get; set; }
    }
}
