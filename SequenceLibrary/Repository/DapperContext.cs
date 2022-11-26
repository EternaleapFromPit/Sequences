using Microsoft.Data.SqlClient;
using SequenceLibrary.Configuration;
using System.Data;
//using System.Data.SqlClient;

namespace SequenceLibrary.Repository
{
    public class DapperContext
    {
        private readonly ISequenceConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(ISequenceConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.ConnectionString;
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
