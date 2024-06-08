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
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            _mockRepo = new Mock<IAuthorRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new AuthorsController(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAuthors_ReturnsOkResult_WithListOfAuthors()
        {
            var authors = new List<Author> { new Author { Id = 1, FirstName = "Test", LastName = "Author" } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors);
            _mockMapper.Setup(m => m.Map<IEnumerable<AuthorResponseDto>>(It.IsAny<IEnumerable<Author>>()))
                       .Returns(new List<AuthorResponseDto> { new AuthorResponseDto { Id = 1, FirstName = "Test", LastName = "Author" } });

            var result = await _controller.GetAllAuthors();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAuthors = Assert.IsType<List<AuthorResponseDto>>(okResult.Value);
            Assert.Single(returnAuthors);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Author)null);

            var result = await _controller.GetAuthorById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsOkResult_WithAuthor()
        {
            var author = new Author { Id = 1, FirstName = "Test", LastName = "Author" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(author);
            _mockMapper.Setup(m => m.Map<AuthorResponseDto>(It.IsAny<Author>()))
                       .Returns(new AuthorResponseDto { Id = 1, FirstName = "Test", LastName = "Author" });

            var result = await _controller.GetAuthorById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAuthor = Assert.IsType<AuthorResponseDto>(okResult.Value);
            Assert.Equal(1, returnAuthor.Id);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAtAction_WithNewAuthor()
        {
            var authorCreateDto = new AuthorRequestDto { FirstName = "New", LastName = "Author", BirthYear = System.DateTime.Now };
            var author = new Author { Id = 1, FirstName = "New", LastName = "Author" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<Author>(It.IsAny<AuthorRequestDto>())).Returns(author);
            _mockMapper.Setup(m => m.Map<AuthorResponseDto>(It.IsAny<Author>()))
                       .Returns(new AuthorResponseDto { Id = 1, FirstName = "New", LastName = "Author" });

            var result = await _controller.CreateAuthor(authorCreateDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnAuthor = Assert.IsType<AuthorResponseDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnAuthor.Id);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var authorUpdateDto = new AuthorRequestDto { FirstName = "Updated", LastName = "Author", BirthYear = System.DateTime.Now };
            var existingAuthor = new Author { Id = 1, FirstName = "Existing", LastName = "Author" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingAuthor);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Author>())).Returns(Task.CompletedTask);

            var result = await _controller.UpdateAuthor(1, authorUpdateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNoContent_WhenDeleteIsSuccessful()
        {
            var existingAuthor = new Author { Id = 1, FirstName = "Existing", LastName = "Author" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(existingAuthor);
            _mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _controller.DeleteAuthor(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
