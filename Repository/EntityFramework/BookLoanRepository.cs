using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using IRepository;

namespace Repository.EntityFramework
{
    public class BookLoanRepository : GenericRepository<BookLoan>, IBookLoanRepository
    {
        private readonly ExempleApiDbContext _context;

        public BookLoanRepository(ExempleApiDbContext context) : base(context)
        {
            _context = context;
        }
    }
}