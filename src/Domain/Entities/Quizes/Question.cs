using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Question : EntityBase
    {
        public Question()
        {
        }

        public Question(string text, bool isMandatory, Guid quizId)
        {
            Text = text;
            IsMandatory = isMandatory;
            QuizId = quizId;
        }

        public string Text { get; set; }
        public bool IsMandatory { get; set; }

        public Guid QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<QuestionOption> QuestionOptions { get; set; }

    }
}
