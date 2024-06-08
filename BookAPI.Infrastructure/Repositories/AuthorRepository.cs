using BookAPI.Application.Interfaces.Repositories;
using BookAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Infrastructure.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Author>> GetAuthorsByBookIdAsync(int bookId)
        {
            return await _context.Authors
                .Include(a => a.BookAuthors)
                .ThenInclude(ba => ba.Book)
                .Where(a => a.BookAuthors.Any(ba => ba.BookId == bookId))
                .ToListAsync();
        }
    }
}
