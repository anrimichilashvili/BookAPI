using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Domain.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        [MinLength(1, ErrorMessage = "The Title must be at least 3 characters long.")]
        [MaxLength(100, ErrorMessage = "The Title cannot exceed 15 characters.")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Url(ErrorMessage = "The Image URL is not valid.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "The Rating field is required.")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "The PublishedDate field is required.")]
        public DateTime PublishedDate { get; set; }
        public bool IsBorrowed { get; set; } = false;

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>(); 

    }
}
