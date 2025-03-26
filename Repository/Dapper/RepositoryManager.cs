using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Repository.Dapper
{
    public class RepositoryManager
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
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

        private void ResetRepositories()
        {

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
    }
}
