using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Entities;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Moq;
using UnitTest.MockData;

namespace UnitTest.MockRepository
{
    public class MockBookRepository
    {
        public static Mock<IBookRepository> GetMock()
        {
            var mockRepo = new Mock<IBookRepository>();
            var mockData = BookData.GetBooks();

            // Setup Mock for FindAllAsync
            mockRepo.Setup(repo => repo.FindAllAsync())
                .ReturnsAsync(mockData);

            // Setup Mock for FindOneByIdAsync
            mockRepo.Setup(repo => repo.FindOneByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => mockData.FirstOrDefault(b => b.Id == id));

            // Setup Mock for FindByConditionAsync
            mockRepo.Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync((Expression<Func<Book, bool>> expression) => {
                    var queryable = mockData.AsQueryable().Where(expression);
                    return queryable;
                });

            // Setup Mock for CreateAsync
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Book>()))
                .Returns(Task.CompletedTask)
                .Callback<Book>(book => {
                    book.Id = mockData.Max(b => b.Id) + 1;
                    mockData.Add(book);
                });

            // Setup Mock for Create (synchronous version)
            mockRepo.Setup(repo => repo.Create(It.IsAny<Book>()))
                .Callback<Book>(book => {
                    book.Id = mockData.Max(b => b.Id) + 1;
                    mockData.Add(book);
                });

            // Setup Mock for Update
            mockRepo.Setup(repo => repo.Update(It.IsAny<Book>()))
                .Callback<Book>(book => {
                    var existingBook = mockData.FirstOrDefault(b => b.Id == book.Id);
                    if (existingBook != null)
                    {
                        int index = mockData.IndexOf(existingBook);
                        mockData[index] = book;
                    }
                });

            // Setup Mock for UpdateAsync
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Book>()))
                .Returns(Task.CompletedTask)
                .Callback<Book>(book => {
                    var existingBook = mockData.FirstOrDefault(b => b.Id == book.Id);
                    if (existingBook != null)
                    {
                        int index = mockData.IndexOf(existingBook);
                        mockData[index] = book;
                    }
                });

            // Setup Mock for Delete
            mockRepo.Setup(repo => repo.Delete(It.IsAny<Book>()))
                .Callback<Book>(book => {
                    var existingBook = mockData.FirstOrDefault(b => b.Id == book.Id);
                    if (existingBook != null)
                    {
                        mockData.Remove(existingBook);
                    }
                });

            // Setup Mock for ExistsAsync
            mockRepo.Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => mockData.Any(b => b.Id == id));

            return mockRepo;
        }
    }
}