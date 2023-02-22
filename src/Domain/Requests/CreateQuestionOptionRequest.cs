namespace Domain.Requests
{
    public class CreateQuestionOptionRequest
    {
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
    }
}
