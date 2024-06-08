using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Application.DTOs
{
    public class BookBorrowReturnDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public bool IsBorrowed { get; set; }
    }
}
