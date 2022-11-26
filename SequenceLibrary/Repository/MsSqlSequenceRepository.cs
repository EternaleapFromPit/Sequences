using Dapper;
using Microsoft.Extensions.Logging;

namespace SequenceLibrary.Repository
{
    /// <summary>
    /// Better to move exact implementation of mssql provider into separate assembly and keed inside the library business logic only
    /// </summary>
    public class MsSqlSequenceRepository : ISequenceRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<MsSqlSequenceRepository> _logger;

        public MsSqlSequenceRepository(DapperContext context, ILogger<MsSqlSequenceRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int?> Read(string sequenceName)
        {
            var query = "SELECT value FROM Sequences where name = @sequenceName";
            
            _logger.LogDebug($"query: {query} with parameters {sequenceName}");
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync(query, new { sequenceName });
                return result?.value;
            }
        }

        public async Task Create(string sequenceName, int value)
        {
            var query = "INSERT INTO Sequences VALUES (@sequenceName, @value)";
            _logger.LogDebug($"query: {query} with parameters {sequenceName}, {value}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync(query, new { sequenceName, value });
            }
        }

        public async Task Delete(string sequenceName)
        {
            var query = "DELETE FROM Sequences where name = @sequenceName";
            _logger.LogDebug($"query: {query} with parameters {sequenceName}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync(query, new { sequenceName });
            }
        }

        public async Task Update(string sequenceName, int value)
        {
            var query = "UPDATE Sequences SET value = @value where name = @sequenceName";
            _logger.LogDebug($"query: {query} with parameters {value}, {sequenceName}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QuerySingleOrDefaultAsync(query, new { value, sequenceName });
            }
        }
    }
}
