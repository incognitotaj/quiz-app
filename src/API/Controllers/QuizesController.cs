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
    [Route("api/[controller]")]
    [ApiController]
    public class QuizesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IQuizRepository _quizRepository;

        public QuizesController(IMapper mapper, IQuizRepository quizRepository)
        {
            _mapper = mapper;
            _quizRepository = quizRepository;
        }


        /// <summary>
        /// Get list of all quizes
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<QuizDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _quizRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<QuizDto>>(result));
        }

        /// <summary>
        /// Get a single specific quiz by it's unique id (GUID)
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpGet("{quizId}")]
        [ProducesResponseType(typeof(QuizDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<QuizDto>> GetById(Guid quizId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            return Ok(_mapper.Map<QuizDto>(quiz));
        }

        /// <summary>
        /// Creates / registers a new quiz
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateQuizRequest request)
        {
            var entity = _mapper.Map<Quiz>(request);

            var newEntity = await _quizRepository.AddAsync(entity);

            return Ok(newEntity.Id);
        }

        /// <summary>
        /// Updates an existing quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{quizId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid quizId, [FromBody] UpdateQuizRequest request)
        {
            var entityToUpdate = await _quizRepository.GetByIdAsync(quizId);
            if (entityToUpdate == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            entityToUpdate.Title = request.Title;
            entityToUpdate.Description = request.Description;

            await _quizRepository.UpdateAsync(entityToUpdate);

            return NoContent();
        }

        /// <summary>
        /// Deletes an existing quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpDelete("{quizId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid quizId)
        {
            var entityToDelete = await _quizRepository.GetByIdAsync(quizId);
            if (entityToDelete == null)
            {
                return NotFound($"Quiz with {quizId} no more exists");
            }

            await _quizRepository.DeleteAsync(entityToDelete);
            return NoContent();
        }
    }
}
