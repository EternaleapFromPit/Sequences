using Dapper;
using Microsoft.Extensions.Logging;
using SequenceLibrary.DTO;

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

        public async Task<SequenceDto> Read(string sequenceName)
        {
            var query = "SELECT value, date FROM Sequences where name = @sequenceName";
            
            _logger.LogDebug($"query: {query} with parameters {sequenceName}");
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync(query, new { sequenceName });
                return new SequenceDto { Value = result?.value, Year = result?.date };
            }
        }

        public async Task Create(string sequenceName, int value, string date)
        {
            var query = "INSERT INTO Sequences VALUES (@sequenceName, @value, @date)";
            _logger.LogDebug($"query: {query} with parameters {sequenceName}, {value}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync(query, new { sequenceName, value, date });
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
