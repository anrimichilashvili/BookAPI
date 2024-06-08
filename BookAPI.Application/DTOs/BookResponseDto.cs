using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Application.DTOs
{
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsBorrowed { get; set; }
        public bool IsDeleted { get; set; } 
        public List<AuthorDto> Authors { get; set; }
    }
}
