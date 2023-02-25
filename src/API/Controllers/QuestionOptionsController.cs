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
    [Route("api/quizes/{quizId}/questions/{questionId}/[controller]")]
    [ApiController]
    public class QuestionOptionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionOptionRepository _questionOptionRepository;

        public QuestionOptionsController(IMapper mapper,
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository,
            IQuestionOptionRepository questionOptionRepository)
        {
            _mapper = mapper;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _questionOptionRepository = questionOptionRepository;
        }


        /// <summary>
        /// Get list of all questione options in a question
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<QuestionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid quizId, Guid questionId)
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

            var result = await _questionOptionRepository.GetByQuestionIdAsync(questionId);
            return Ok(_mapper.Map<IEnumerable<QuestionOptionDto>>(result));
        }

        /// <summary>
        /// Get a single specific question optoin by it's unique id (GUID)
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="questionOptionId"></param>
        /// <returns></returns>
        [HttpGet("{questionOptionId}")]
        [ProducesResponseType(typeof(QuestionDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<QuestionDto>> GetById(Guid quizId, Guid questionId, Guid questionOptionId)
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

            var questionOption = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (questionOption == null)
            {
                return NotFound($"Question option with {questionOptionId} no more exists");
            }

            return Ok(_mapper.Map<QuestionOptionDto>(questionOption));
        }

        /// <summary>
        /// Creates / registers a new question option
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> Create(Guid quizId, Guid questionId, [FromBody] CreateQuestionOptionRequest request)
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

            var entity = _mapper.Map<QuestionOption>(request);
            entity.QuestionId = questionId;

            var newEntity = await _questionOptionRepository.AddAsync(entity);

            return Ok(newEntity.Id);
        }

        /// <summary>
        /// Updates an existing question option
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="questionOptionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{questionOptionId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid quizId, Guid questionId, Guid questionOptionId, [FromBody] UpdateQuestionOptionRequest request)
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


            var entityToUpdate = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (entityToUpdate == null)
            {
                return NotFound($"Question option with {questionOptionId} no more exists");
            }
            entityToUpdate.Text = request.Text;
            entityToUpdate.IsAnswer = request.IsAnswer;

            await _questionOptionRepository.UpdateAsync(entityToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing question option
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="questionOptionId"></param>
        /// <returns></returns>
        [HttpDelete("{questionOptionId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid quizId, Guid questionId, Guid questionOptionId)
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

            var entityToDelete = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (entityToDelete == null)
            {
                return NotFound($"Question option with {questionOptionId} no more exists");
            }

            await _questionOptionRepository.DeleteAsync(entityToDelete);
            return NoContent();
        }
    }
}
