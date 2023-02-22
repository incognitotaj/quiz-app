using Domain.Common;

namespace Domain.Entities
{
    public class Quiz : EntityBase
    {
        public Quiz()
        {
        }

        public Quiz(string title, string description)
        {
            Title = title;
            Description = description;

            Questions = new HashSet<Question>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

    }
}
