namespace SequenceLibrary.Sequences
{
    public interface ISequence
    {
        public string GetCurrent();

        public Task<string> GetNext();
    }
}
