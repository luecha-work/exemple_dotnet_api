using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace IRepository
{
    public interface IBookRepository : IGenericRepositoryEntityFramework<Book> { }
}