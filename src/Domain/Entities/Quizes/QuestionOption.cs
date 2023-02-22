using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class QuestionOption : EntityBase
    {
        public QuestionOption()
        {
        }

        public QuestionOption(Guid questionId, string text, bool isAnswer)
        {
            QuestionId = questionId;
            Text = text;
            IsAnswer = isAnswer;
        }

        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string Text { get; set; }
        public bool IsAnswer { get; set; }
    }
}
