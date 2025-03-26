using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using IRepository;

namespace Repository.EntityFramework
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly ExempleApiDbContext _context;

        public BookRepository(ExempleApiDbContext context) : base(context)
        {
            _context = context;
        }
    }
}