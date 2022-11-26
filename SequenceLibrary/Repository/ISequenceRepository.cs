using SequenceLibrary.DTO;

namespace SequenceLibrary.Repository
{
    /// <summary>
    /// sequences db operation
    /// </summary>
    public interface ISequenceRepository
    {
        public Task<SequenceDto> Read(string sequenceName);

        public Task Create(string sequenceName, int value, string date);

        public Task Update(string sequenceName, int value);
    }
}
