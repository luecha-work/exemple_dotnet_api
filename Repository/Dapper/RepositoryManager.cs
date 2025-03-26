using IRepository;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Dapper
{
    public class RepositoryManager : IRepositoryManager, IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private IAccountsRepository _accountRepository;
        private IRoleRepository _roleRepository;
        private IAccountRolesRepository _accountRolesRepository;
        private IAuthenticationManager _authenticationManager;
        private ISystemSessionRepository _systemSessionRepository;
        private IBlockBruteForceRepository _blockBruteForceRepository;
        private IBookLoanRepository _bookLoanRepository;
        private IBookRepository _bookRepository;

        private bool _disposed;


        public RepositoryManager(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (connectionString == null)
                throw new ArgumentException("Connection string 'DefaultConnection' not found.");
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IAccountsRepository AccountRepository
        {
            get { return _accountRepository ??= new AccountRepository(_transaction); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ??= new RoleRepository(_transaction); }
        }

        public IAccountRolesRepository AccountRolesRepository
        {
            get { return _accountRolesRepository ??= new AccountRolesRepository(_transaction); }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get { return _authenticationManager ??= new AuthenticationManager(_transaction); }
        }

        public ISystemSessionRepository SystemSessionRepository
        {
            get { return _systemSessionRepository ??= new SystemSessionRepository(_transaction); }
        }

        public IBlockBruteForceRepository BlockBruteForceRepository
        {
            get { return _blockBruteForceRepository ??= new BlockBruteForceRepository(_transaction); }
        }

        public IBookLoanRepository BookLoanRepository
        {
            get { return _bookLoanRepository ??= new BookLoanRepository(_transaction); }
        }

        public IBookRepository BookRepository
        {
            get { return _bookRepository ??= new BookRepository(_transaction); }
        }
        private void ResetRepositories()
        {
            _accountRepository = null;
            _roleRepository = null;
            _accountRolesRepository = null;
            _authenticationManager = null;
            _systemSessionRepository = null;
            _blockBruteForceRepository = null;
            _bookLoanRepository = null;
            _bookRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                    _connection.Dispose();
                }
                _disposed = true;
            }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }
    }
}
