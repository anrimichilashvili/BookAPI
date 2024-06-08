using AutoMapper;
using BookAPI.Application.DTOs;
using BookAPI.Domain.Models;
using System.Linq;

namespace BookAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<BookCreateDto, Book>()
                .ForMember(dest => dest.BookAuthors, opt => opt.MapFrom(src => src.AuthorIds.Select(id => new BookAuthor { AuthorId = id })));

            CreateMap<BookUpdateDto, Book>()
                .ForMember(dest => dest.BookAuthors, opt => opt.MapFrom(src => src.AuthorIds.Select(id => new BookAuthor { AuthorId = id })));


            CreateMap<Book, BookResponseDto>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src =>
                    src.BookAuthors != null
                    ? src.BookAuthors.Select(ba => new AuthorDto
                    {
                        Id = ba.Author.Id,
                        FullName = $"{ba.Author.FirstName} {ba.Author.LastName}"
                    }).ToList()
                    : new List<AuthorDto>()
                ));


            CreateMap<AuthorRequestDto, Author>();


            CreateMap<Author, AuthorResponseDto>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src =>
                    src.BookAuthors != null
                    ? src.BookAuthors.Select(ba => new BookDto
                    {
                        Id = ba.Book.Id,
                        Title = ba.Book.Title
                    }).ToList()
                    : new List<BookDto>()));


            CreateMap<Book, BookDto>(); 


            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        }
    }
}
