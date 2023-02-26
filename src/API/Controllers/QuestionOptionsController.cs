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
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(Guid quizId, Guid questionId)
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

            var result = await _questionOptionRepository.GetByQuestionIdAsync(questionId);
            return Ok(new ApiResponse(200, null, _mapper.Map<IEnumerable<QuestionOptionDto>>(result)));

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
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<QuestionDto>> GetById(Guid quizId, Guid questionId, Guid questionOptionId)
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

            var questionOption = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (questionOption == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(new ApiResponse(200, null, _mapper.Map<QuestionOptionDto>(questionOption)));

        }

        /// <summary>
        /// Creates / registers a new question option
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Guid>> Create(Guid quizId, Guid questionId, [FromBody] CreateQuestionOptionRequest request)
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

            var entity = _mapper.Map<QuestionOption>(request);
            entity.QuestionId = questionId;

            var newEntity = await _questionOptionRepository.AddAsync(entity);

            return Created($"/api/quizes/{quizId}/questions/{questionId}/questionOptions/{newEntity.Id}", new ApiResponse(201, null, _mapper.Map<QuestionOptionDto>(newEntity)));

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
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid quizId, Guid questionId, Guid questionOptionId, [FromBody] UpdateQuestionOptionRequest request)
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


            var entityToUpdate = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (entityToUpdate == null)
            {
                return NotFound(new ApiResponse(404));
            }
            entityToUpdate.Text = request.Text;
            entityToUpdate.IsAnswer = request.IsAnswer;

            await _questionOptionRepository.UpdateAsync(entityToUpdate);

            return Ok(new ApiResponse(204, null, _mapper.Map<QuestionOptionDto>(entityToUpdate)));
        }

        /// <summary>
        /// Deletes an existing question option
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="questionId"></param>
        /// <param name="questionOptionId"></param>
        /// <returns></returns>
        [HttpDelete("{questionOptionId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid quizId, Guid questionId, Guid questionOptionId)
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

            var entityToDelete = await _questionOptionRepository.GetByIdAsync(questionOptionId);
            if (entityToDelete == null)
            {
                return NotFound(new ApiResponse(404));
            }

            await _questionOptionRepository.DeleteAsync(entityToDelete);
            return Ok(new ApiResponse(204, null, null));
        }
    }
}
