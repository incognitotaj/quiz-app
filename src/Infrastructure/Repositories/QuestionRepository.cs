using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Contracts.Persistence;

namespace Infrastructure.Repositories
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        public QuestionRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Question>> GetByQuizIdAsync(Guid quizId)
        {
            var temp = await _context.Questions
                                    .Where(p => p.QuizId == quizId)
                                    .ToListAsync();

            return temp;
        }
    }
}
