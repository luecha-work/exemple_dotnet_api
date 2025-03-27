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
    public class MockBookLoanRepository
    {
        public static Mock<IBookLoanRepository> GetMock()
        {
            var mockRepo = new Mock<IBookLoanRepository>();
            var mockData = BookLoanData.GetBookLoans();

            // Setup Mock for FindAllAsync
            mockRepo.Setup(repo => repo.FindAllAsync())
                .ReturnsAsync(mockData);

            // Setup Mock for FindOneByIdAsync
            mockRepo.Setup(repo => repo.FindOneByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => mockData.FirstOrDefault(b => b.Id == id));

            // Setup Mock for FindByConditionAsync
            mockRepo.Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<BookLoan, bool>>>()))
                .ReturnsAsync((Expression<Func<BookLoan, bool>> expression) => {
                    var queryableData = mockData.AsQueryable().Where(expression);
                    return queryableData;
                });

            // Setup Mock for Create
            mockRepo.Setup(repo => repo.Create(It.IsAny<BookLoan>()))
                .Callback<BookLoan>(loan => {
                    loan.Id = mockData.Count > 0 ? mockData.Max(b => b.Id) + 1 : 1;
                    mockData.Add(loan);
                });

            // Setup Mock for CreateAsync
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<BookLoan>()))
                .Returns(Task.CompletedTask)
                .Callback<BookLoan>(loan => {
                    loan.Id = mockData.Count > 0 ? mockData.Max(b => b.Id) + 1 : 1;
                    mockData.Add(loan);
                });

            // Setup Mock for Update
            mockRepo.Setup(repo => repo.Update(It.IsAny<BookLoan>()))
                .Callback<BookLoan>(loan => {
                    var existingLoan = mockData.FirstOrDefault(b => b.Id == loan.Id);
                    if (existingLoan != null)
                    {
                        var index = mockData.IndexOf(existingLoan);
                        mockData[index] = loan;
                    }
                });

            // Setup Mock for UpdateAsync
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<BookLoan>()))
                .Returns(Task.CompletedTask)
                .Callback<BookLoan>(loan => {
                    var existingLoan = mockData.FirstOrDefault(b => b.Id == loan.Id);
                    if (existingLoan != null)
                    {
                        var index = mockData.IndexOf(existingLoan);
                        mockData[index] = loan;
                    }
                });

            // Setup Mock for Delete
            mockRepo.Setup(repo => repo.Delete(It.IsAny<BookLoan>()))
                .Callback<BookLoan>(loan => {
                    var existingLoan = mockData.FirstOrDefault(b => b.Id == loan.Id);
                    if (existingLoan != null)
                    {
                        mockData.Remove(existingLoan);
                    }
                });

            // Setup Mock for ExistsAsync
            mockRepo.Setup(repo => repo.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => mockData.Any(b => b.Id == id));

            return mockRepo;
        }
    }
}