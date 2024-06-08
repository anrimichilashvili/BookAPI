using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Application.DTOs
{
    public class AuthorResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthYear { get; set; }
        public bool IsDeleted { get; set; } 
        public List<BookDto> Books { get; set; } 
    }

    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
