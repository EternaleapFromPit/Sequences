using Dapper;

namespace SequenceLibrary.Repository
{
    /// <summary>
    /// Better to move exact implementation of mssql provider into separate assembly and keed inside the library business logic only
    /// </summary>
    public class MsSqlSequenceRepository : ISequenceRepository
    {
        private readonly DapperContext _context;

        public MsSqlSequenceRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int?> Read(string sequenceName)
        {
            var query = "SELECT value FROM Sequences where name = @sequenceName";
            Console.WriteLine($"query: {query}");
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync(query, new { sequenceName });
                return result?.value;
            }
        }

        public async Task Create(string sequenceName, int value)
        {
            var query = "INSERT INTO Sequences VALUES (@sequenceName, @value)";
            Console.WriteLine($"query: {query}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync(query, new { sequenceName, value });
            }
        }

        public async Task Delete(string sequenceName)
        {
            var query = "DELETE FROM Sequences where name = @sequenceName";
            Console.WriteLine($"query: {query}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QueryAsync(query, new { sequenceName });
            }
        }

        public async Task Update(string sequenceName, int value)
        {
            var query = "UPDATE Sequences SET value = @value where name = @sequenceName";
            Console.WriteLine($"query: {query}");
            using (var connection = _context.CreateConnection())
            {
                await connection.QuerySingleOrDefaultAsync(query, new { value, sequenceName });
            }
        }
    }
}
