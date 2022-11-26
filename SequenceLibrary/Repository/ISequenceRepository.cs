namespace SequenceLibrary.Repository
{
    /// <summary>
    /// sequences CRUD
    /// </summary>
    public interface ISequenceRepository
    {
        public Task<int?> Read(string sequenceName);

        public Task Create(string sequenceName, int value);

        public Task Update(string sequenceName, int value);

        public Task Delete(string sequenceName);
    }
}
