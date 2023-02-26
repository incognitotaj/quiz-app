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
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _quizRepository.GetAllAsync();
            return Ok(new ApiResponse(200, null, _mapper.Map<IEnumerable<QuizDto>>(result)));

        }

        /// <summary>
        /// Get a single specific quiz by it's unique id (GUID)
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpGet("{quizId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse),(int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<QuizDto>> GetById(Guid quizId)
        {
            var quiz = await _quizRepository.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(new ApiResponse(200, null, _mapper.Map<QuizDto>(quiz)));
        }

        /// <summary>
        /// Creates / registers a new quiz
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateQuizRequest request)
        {
            var entity = _mapper.Map<Quiz>(request);

            var newEntity = await _quizRepository.AddAsync(entity);

            return Created($"/api/quizes/{newEntity.Id}", new ApiResponse(201, null, _mapper.Map<QuizDto>(newEntity)));
        }

        /// <summary>
        /// Updates an existing quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{quizId}")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(Guid quizId, [FromBody] UpdateQuizRequest request)
        {
            var entityToUpdate = await _quizRepository.GetByIdAsync(quizId);
            if (entityToUpdate == null)
            {
                return NotFound(new ApiResponse(404));
            }

            entityToUpdate.Title = request.Title;
            entityToUpdate.Description = request.Description;

            await _quizRepository.UpdateAsync(entityToUpdate);

            return Ok(new ApiResponse(204, null, _mapper.Map<QuizDto>(entityToUpdate)));
        }

        /// <summary>
        /// Deletes an existing quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns></returns>
        [HttpDelete("{quizId}")]
        [ProducesResponseType(typeof(ApiResponse),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(Guid quizId)
        {
            var entityToDelete = await _quizRepository.GetByIdAsync(quizId);
            if (entityToDelete == null)
            {
                return NotFound(new ApiResponse(404));
            }

            await _quizRepository.DeleteAsync(entityToDelete);
            return Ok(new ApiResponse(204, null, null));

        }
    }
}
