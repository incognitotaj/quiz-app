using AutoMapper;
using Application.Dtos;
using Domain.Entities;
using Domain.Requests;

namespace Team.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Quiz, QuizDto>().ReverseMap();
            CreateMap<Quiz, CreateQuizRequest>().ReverseMap();
            CreateMap<Quiz, UpdateQuizRequest>().ReverseMap();


            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, CreateQuestionRequest>().ReverseMap();
            CreateMap<Question, UpdateQuestionRequest>().ReverseMap();


            CreateMap<QuestionOption, QuestionOptionDto>().ReverseMap();
            CreateMap<QuestionOption, CreateQuestionOptionRequest>().ReverseMap();
            CreateMap<QuestionOption, UpdateQuestionOptionRequest>().ReverseMap();
        }
    }
}
