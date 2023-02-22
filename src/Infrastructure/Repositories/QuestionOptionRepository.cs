using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Contracts.Persistence;

namespace Infrastructure.Repositories
{
    public class QuestionOptionRepository : RepositoryBase<QuestionOption>, IQuestionOptionRepository
    {
        public QuestionOptionRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<QuestionOption>> GetByQuestionIdAsync(Guid questionId)
        {
            var temp = await _context.QuestionOptions
                                    .Where(p => p.QuestionId == questionId)
                                    .ToListAsync();

            return temp;
        }
    }
}
