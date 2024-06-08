using AutoMapper;
using BookAPI.Application.DTOs;
using BookAPI.Application.Interfaces.Repositories;
using BookAPI.Controllers;
using BookAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BookAPI.Tests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            _mockRepo = new Mock<IBookRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new BooksController(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
        {
            var books = new List<Book> { new Book { Id = 1, Title = "Test Book" } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(books);
            _mockMapper.Setup(m => m.Map<IEnumerable<BookResponseDto>>(It.IsAny<IEnumerable<Book>>()))
                       .Returns(new List<BookResponseDto> { new BookResponseDto { Id = 1, Title = "Test Book" } });

            var result = await _controller.GetAllBooks();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBooks = Assert.IsType<List<BookResponseDto>>(okResult.Value);
            Assert.Single(returnBooks);
        }

        [Fact]
        public async Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Book)null);

            var result = await _controller.GetBookById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetBookById_ReturnsOkResult_WithBook()
        {
            var book = new Book { Id = 1, Title = "Test Book" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(book);
            _mockMapper.Setup(m => m.Map<BookResponseDto>(It.IsAny<Book>()))
                       .Returns(new BookResponseDto { Id = 1, Title = "Test Book" });

            var result = await _controller.GetBookById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBook = Assert.IsType<BookResponseDto>(okResult.Value);
            Assert.Equal(1, returnBook.Id);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedAtAction_WithNewBook()
        {
            var bookCreateDto = new BookCreateDto { Title = "New Book", Rating = 4.5, PublishedDate = System.DateTime.Now, AuthorIds = new List<int> { 1 } };
            var book = new Book { Id = 1, Title = "New Book" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<Book>(It.IsAny<BookCreateDto>())).Returns(book);
            _mockMapper.Setup(m => m.Map<BookResponseDto>(It.IsAny<Book>()))
                       .Returns(new BookResponseDto { Id = 1, Title = "New Book" });

            var result = await _controller.CreateBook(bookCreateDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnBook = Assert.IsType<BookResponseDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnBook.Id);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var bookUpdateDto = new BookUpdateDto { Id = 1, Title = "Updated Book" };
            var existingBook = new Book { Id = 1, Title = "Existing Book" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingBook);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

            var result = await _controller.UpdateBook(1, bookUpdateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            var existingBook = new Book { Id = 1, Title = "Existing Book" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingBook);
            _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.DeleteBook(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
