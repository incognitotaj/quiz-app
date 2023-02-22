using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Contracts.Persistence
{
    public interface IQuestionRepository : IAsyncRepository<Question>
    {
        // Get Documents by ProjectId
        Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId);
    }
}
