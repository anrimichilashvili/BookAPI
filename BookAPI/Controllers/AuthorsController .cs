using AutoMapper;
using BookAPI.Application.DTOs;
using BookAPI.Application.Interfaces.Repositories;
using BookAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorResponseDto>>> GetAllAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();
            var authorDtos = _mapper.Map<IEnumerable<AuthorResponseDto>>(authors);
            return Ok(authorDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponseDto>> GetAuthorById(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            var authorDto = _mapper.Map<AuthorResponseDto>(author);
            return Ok(authorDto);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorResponseDto>> CreateAuthor([FromBody] AuthorRequestDto authorRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = _mapper.Map<Author>(authorRequestDto);
            await _authorRepository.AddAsync(author);
            var authorDto = _mapper.Map<AuthorResponseDto>(author);

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, authorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorRequestDto authorRequestDto)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            _mapper.Map(authorRequestDto, existingAuthor);
            await _authorRepository.UpdateAsync(existingAuthor);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            await _authorRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
