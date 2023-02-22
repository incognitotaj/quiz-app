using Application.Contracts.Persistence;
using Application.Dtos;
using Application.Exceptions;
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
        [ProducesResponseType(typeof(IEnumerable<QuestionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid quizId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            var result = await _questionRepository.GetByQuizIdAsync(quizId);
            return Ok(_mapper.Map<IEnumerable<QuestionDto>>(result));
        }

        /// <summary>
        /// Get a single specific question by it's unique id (GUID)
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet("{questionId}")]
        [ProducesResponseType(typeof(QuestionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<QuestionDto>> GetById(Guid quizId, Guid questionId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                return NotFound($"Question with {questionId} no more exists");
            }

            return Ok(_mapper.Map<QuestionDto>(question));
        }

        /// <summary>
        /// Creates / registers a new question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> Create(Guid quizId, [FromBody] CreateQuestionRequest request)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            var entity = _mapper.Map<Question>(request);
            entity.QuizId = quizId;

            var newEntity = await _questionRepository.AddAsync(entity);

            return Ok(newEntity.Id);
        }

        /// <summary>
        /// Updates an existing question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{questionId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid quizId, Guid questionId, [FromBody] UpdateQuestionRequest request)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            var entityToUpdate = await _questionRepository.GetByIdAsync(questionId);
            if (entityToUpdate == null)
            {
                return NotFound($"Question with {questionId} no more exists");
            }

            entityToUpdate.Text= request.Text;
            entityToUpdate.IsMandatory= request.IsMandatory;

            await _questionRepository.UpdateAsync(entityToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpDelete("{questionId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid quizId, Guid questionId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            var entityToDelete = await _questionRepository.GetByIdAsync(questionId);
            if (entityToDelete == null)
            {
                return NotFound($"Question with {questionId} no more exists");
            }

            await _questionRepository.DeleteAsync(entityToDelete);
            return NoContent();
        }
    }
}
