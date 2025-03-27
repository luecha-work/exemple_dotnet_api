using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace UnitTest.MockData
{
    public class BookData
    {
        public static List<Book> GetBooks()
        {
            return new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    Isbn = "9780132350884",
                    PublicationYear = 2008,
                    Category = "Programming",
                    Status = "Available",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new Book
                {
                    Id = 2,
                    Title = "Design Patterns",
                    Author = "Erich Gamma",
                    Isbn = "9780201633610",
                    PublicationYear = 1994,
                    Category = "Programming",
                    Status = "Available",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-25)
                },
                new Book
                {
                    Id = 3,
                    Title = "The Pragmatic Programmer",
                    Author = "Andrew Hunt",
                    Isbn = "9780201616224",
                    PublicationYear = 1999,
                    Category = "Programming",
                    Status = "Borrowed",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-20),
                    UpdatedBy = "System",
                    UpdatedAt = DateTime.Now.AddDays(-5)
                },
                new Book
                {
                    Id = 4,
                    Title = "Refactoring",
                    Author = "Martin Fowler",
                    Isbn = "9780201485677",
                    PublicationYear = 1999,
                    Category = "Programming",
                    Status = "Available",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-15)
                },
                new Book
                {
                    Id = 5,
                    Title = "Domain-Driven Design",
                    Author = "Eric Evans",
                    Isbn = "9780321125217",
                    PublicationYear = 2003,
                    Category = "Software Architecture",
                    Status = "Available",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-10)
                }
            };
        }
    }
}