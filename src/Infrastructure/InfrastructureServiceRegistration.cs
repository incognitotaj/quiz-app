using Microsoft.Extensions.DependencyInjection;
using Application.Contracts.Persistence;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

            services.AddScoped<IQuizRepository, QuizRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();

            return services;
        }
    }
}