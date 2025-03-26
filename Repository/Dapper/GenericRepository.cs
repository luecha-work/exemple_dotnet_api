using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Dapper;
using IRepository;

namespace Repository.Dapper
{
    public class GenericRepository<T> : IGenericRepositoryDapper<T>
        where T : class
    {
        protected IDbTransaction _transaction { get; private set; }
        protected IDbConnection _connection
        {
            get { return _transaction.Connection; }
        }
        private readonly string _tableName;

        protected GenericRepository(IDbTransaction transaction)
        {
            _transaction = transaction;
            _tableName = ConvertToDbColumnName(typeof(T).Name);
        }

        public T FindOneById(int id)
        {
            var properties = GenerateListOfProperties()
                .Select(prop => $"{GetColumnName(prop.Name)} AS {prop.Name}");
            var columns = string.Join(", ", properties);
            var sql = string.Format("SELECT {0} FROM {1} WHERE Id = @Id", columns, _tableName);
            var result = _connection.QuerySingleOrDefault<T>(sql, new { Id = id });
            return result;
        }

        public IEnumerable<T> FindAll()
        {
            var properties = GenerateListOfProperties()
                .Select(prop => $"{GetColumnName(prop.Name)} AS {prop.Name}");
            var columns = string.Join(", ", properties);
            var sql = string.Format("SELECT {0} FROM {1}", columns, _tableName);
            var result = _connection.Query<T>(sql);
            return result.ToList();
        }

        public IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            //TODO: Ex use => var result = repository.FindByCondition(x => x.Name == "ExampleName");
            var queryBuilder = new StringBuilder($"SELECT * FROM {_tableName} WHERE ");
            var parameters = new DynamicParameters();

            // Convert Expression to SQL Where Clause
            var whereClause = ConvertExpressionToSqlWhereClause(expression, parameters);
            queryBuilder.Append(whereClause);
            var sql = queryBuilder.ToString();
            var result = _connection.Query<T>(sql, parameters, _transaction);
            return result;
        }

        public int Create(T entity)
        {
            var insertQuery = GenerateInsertQuery();
            var result = _connection.Execute(insertQuery, entity, _transaction);
            return result;
        }

        public int CreateWithOutputId(T entity)
        {
            var insertQuery = new StringBuilder(GenerateInsertQuery());
            insertQuery.Append(" RETURNING id");
            var result = _connection.ExecuteScalar<int>(
                insertQuery.ToString(),
                entity,
                _transaction
            );
            return result;
        }

        public int Update(T entity)
        {
            var updateQuery = GenerateUpdateQuery();
            var result = _connection.Execute(updateQuery, entity, _transaction);
            return result;
        }

        public int Delete(int id)
        {
            var sql = string.Format("DELETE FROM {0} WHERE Id = @Id", _tableName);
            var result = _connection.Execute(sql, new { Id = id }, _transaction);
            return result;
        }

        public int ExecuteSqlCommand(string sql, object parameters = null)
        {
            //TODO: Ex use for  INSERT, UPDATE Or DELETE
            // var sql = "UPDATE MyTable SET Column1 = @Value1 WHERE Id = @Id";
            // var parameters = new { Value1 = "NewValue", Id = 1 };
            // repository.ExecuteSqlCommand(sql, parameters);

            return _connection.Execute(sql, parameters, _transaction);
        }

        public IEnumerable<T> QuerySqlCommand(string sql, object parameters = null)
        {
            //TODO: Ex use for SELECT
            // var sql = "SELECT * FROM MyTable WHERE Column1 = @Value1";
            // var parameters = new { Value1 = "SomeValue" };
            // var result = repository.QuerySqlCommand(sql, parameters);

            return _connection.Query<T>(sql, parameters, _transaction);
        }

        // PRIVATE
        private static string GetColumnName(string propertyName)
        {
            // add an underscore before each uppercase letter that is followed by a lowercase letter,
            // then make the entire string uppercase
            return new string(
                propertyName
                    .SelectMany(
                        (c, i) =>
                            i > 0 && char.IsLower(propertyName[i - 1]) && char.IsUpper(c)
                                ? new[] { '_', c }
                                : new[] { c }
                    )
                    .ToArray()
            ).ToUpper();
        }

        private static Func<PropertyInfo, bool> GetIsNotIdentityFunc()
        {
            return p => !p.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase);
        }

        private static List<PropertyInfo> GenerateListOfProperties(
            Func<PropertyInfo, bool> selector = null
        )
        {
            var properties = typeof(T).GetProperties();

            if (selector != null)
                properties = properties.Where(selector).ToArray();

            var simpleTypes = new[]
            {
                typeof(string),
                typeof(DateTime),
                typeof(DateTime?),
                typeof(int),
                typeof(int?),
                typeof(long),
                typeof(long?),
                typeof(decimal),
                typeof(decimal?),
                typeof(double),
                typeof(double?),
                // add more types if needed
            };

            return properties.Where(p => simpleTypes.Contains(p.PropertyType)).ToList();
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append('(');

            var properties = GenerateListOfProperties(GetIsNotIdentityFunc());

            properties.ForEach(prop =>
            {
                insertQuery.Append($"{ConvertToDbColumnName(prop.Name)},");
            });

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(") VALUES (");

            properties.ForEach(prop =>
            {
                insertQuery.Append($"@{prop.Name},");
            });

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(')');

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetIsNotIdentityFunc());

            properties.ForEach(prop =>
            {
                updateQuery.Append($"{ConvertToDbColumnName(prop.Name)} = @{prop.Name},");
            });

            updateQuery.Remove(updateQuery.Length - 1, 1);
            updateQuery.Append($" WHERE {ConvertToDbColumnName("Id")} = @Id");

            return updateQuery.ToString();
        }

        private static string ConvertToDbColumnName(string propertyName)
        {
            var result = new StringBuilder();
            var letters = propertyName.ToCharArray();

            foreach (var letter in letters)
            {
                if (char.IsUpper(letter) && result.Length > 0)
                {
                    result.Append('_');
                }

                result.Append(letter);
            }

            return result.ToString().ToLower();
        }

        private static string ConvertExpressionToSqlWhereClause(
            Expression<Func<T, bool>> expression,
            DynamicParameters parameters
        )
        {
            // This is a simplified example, a real implementation will need a robust way
            // to parse the expression and convert it into SQL query and parameters
            if (expression.Body is BinaryExpression binaryExpression)
            {
                var left = binaryExpression.Left as MemberExpression;
                var right = binaryExpression.Right as ConstantExpression;
                var parameterName = left.Member.Name;
                var parameterValue = right.Value;

                parameters.Add(parameterName, parameterValue);

                return $"{GetColumnName(parameterName)} = @{parameterName}";
            }

            throw new NotSupportedException(
                "Only simple binary expressions are supported in this example."
            );
        }
    }
}