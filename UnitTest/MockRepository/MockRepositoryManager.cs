using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRepository;
using Moq;

namespace UnitTest.MockRepository
{
    public class MockRepositoryManager
    {
        public static Mock<IRepositoryManager> GetMock()
        {
            var mockRepo = new Mock<IRepositoryManager>();
            var BookLoanRepository = MockBookLoanRepository.GetMock();
            var BookRepository = MockBookRepository.GetMock();


            //Setup Mock
            mockRepo
                .Setup(m => m.BookLoanRepository)
                .Returns(() => BookLoanRepository.Object);

            mockRepo
                .Setup(m => m.BookRepository)
                .Returns(() => BookRepository.Object);

            return mockRepo;
        }
    }
}