using Application.Contracts.Persistence;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class QuizRepository : RepositoryBase<Quiz>, IQuizRepository
    {
        public QuizRepository(DataContext context)
            : base(context)
        {
        }
    }
}
