using API.Errors;
using Application.Contracts.Persistence;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/quizes/{quizId}/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IMapper mapper,
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository)
        {
            _mapper = mapper;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
        }


        /// <summary>
        /// Get list of all questiones in Quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(Guid quizId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var result = await _questionRepository.GetByQuizIdAsync(quizId);
            return Ok(new ApiResponse(200, null, _mapper.Map<IEnumerable<QuestionDto>>(result)));
        }

        /// <summary>
        /// Get a single specific question by it's unique id (GUID)
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<QuestionDto>> GetById(Guid quizId, Guid questionId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(new ApiResponse(200, null, _mapper.Map<QuestionDto>(question)));
        }

        /// <summary>
        /// Creates / registers a new question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Guid>> Create(Guid quizId, [FromBody] CreateQuestionRequest request)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var entity = _mapper.Map<Question>(request);
            entity.QuizId = quizId;

            var newEntity = await _questionRepository.AddAsync(entity);

            return Created($"/api/quizes/{quizId}/questions/{newEntity.Id}", new ApiResponse(201, null, _mapper.Map<QuestionDto>(newEntity)));

        }

        /// <summary>
        /// Updates an existing question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid quizId, Guid questionId, [FromBody] UpdateQuestionRequest request)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var entityToUpdate = await _questionRepository.GetByIdAsync(questionId);
            if (entityToUpdate == null)
            {
                return NotFound(new ApiResponse(404));
            }

            entityToUpdate.Text = request.Text;
            entityToUpdate.IsMandatory = request.IsMandatory;

            await _questionRepository.UpdateAsync(entityToUpdate);

            return Ok(new ApiResponse(204, null, _mapper.Map<QuestionDto>(entityToUpdate)));

        }

        /// <summary>
        /// Deletes an existing question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpDelete("{questionId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid quizId, Guid questionId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var entityToDelete = await _questionRepository.GetByIdAsync(questionId);
            if (entityToDelete == null)
            {
                return NotFound(new ApiResponse(404));
            }

            await _questionRepository.DeleteAsync(entityToDelete);
            return Ok(new ApiResponse(204, null, null));
        }
    }
}
