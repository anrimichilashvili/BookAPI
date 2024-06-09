using AutoMapper;
using BookAPI.Application.DTOs;
using BookAPI.Application.Interfaces.Repositories;
using BookAPI.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllAsync();
            var bookDtos = _mapper.Map<IEnumerable<BookResponseDto>>(books);

            //უნდა გამოჩნდეს ყველა ავტორი რაც ამ წიგნს ჰყოლია
            //foreach (var bookDto in bookDtos)
            //{
            //    bookDto.Authors = bookDto.Authors?.Where(a => !a.IsDeleted).ToList();
            //}

            return Ok(bookDtos);
        }

        [HttpPatch("{id}/borrow")]
        public async Task<IActionResult> BorrowBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsBorrowed = true;
            await _bookRepository.UpdateAsync(book);

            return NoContent();
        }

        [HttpPatch("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.IsBorrowed = false;
            await _bookRepository.UpdateAsync(book);

            return NoContent();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseDto>> GetBookById(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookResponseDto>(book);

            //bookDto.Authors = bookDto.Authors?.Where(a => !a.IsDeleted).ToList();

            return Ok(bookDto);
        }


        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> CreateBook([FromBody] BookCreateDto bookCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookCreateDto);
            await _bookRepository.AddAsync(book);
            var bookDto = _mapper.Map<BookResponseDto>(book);

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDto bookUpdateDto)
        {
            if (id != bookUpdateDto.Id)
            {
                return BadRequest("Book ID mismatch");
            }

            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            _mapper.Map(bookUpdateDto, existingBook);
            await _bookRepository.UpdateAsync(existingBook);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            await _bookRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
