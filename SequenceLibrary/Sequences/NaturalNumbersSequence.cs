using SequenceLibrary.Repository;
using System.Security.Policy;

namespace SequenceLibrary.Sequences
{
    public class NaturalNumbersSequence : ISequence
    {
        private int _currentValue;
        private int _startValue;
        private int _endValue;
        private ISequenceRepository _repository;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public const string Name = "NaturalNumbers";

        public NaturalNumbersSequence(string range, ISequenceRepository repository)
        {
            if (string.IsNullOrWhiteSpace(range))
                throw new ArgumentException("range is empty");

            if (repository is null)
                throw new ArgumentException("repository instance for the sequence does not exist");

            _repository = repository;

            var rangeElems = range.Split('[', ']').Where(x => x != "").First().Split('.').Where(x => x != "");
            _startValue = Convert.ToInt32(rangeElems.First());
            _endValue = Convert.ToInt32(rangeElems.Last());

            if (string.IsNullOrWhiteSpace(range))
            {
                _startValue = 0;
                _endValue = int.MaxValue;
            }

            var storedValue = _repository.Read(Name)?.Result;
            _currentValue = storedValue?.Value ?? _startValue;

            if (storedValue?.Value == null)
                repository.Create(Name, _currentValue, DateTime.Now.Year.ToString());
            else if (DateTime.Now.Year.ToString() != storedValue.Year) //year had change hence restarting the sequence (date could be changed in database)
                _currentValue = _startValue;
        }

        public string GetCurrent() => _currentValue.ToString();

        public async Task<string> GetNext()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                _currentValue++;
                if (_currentValue > _endValue)
                    _currentValue = _startValue;
                await _repository.Update(Name, _currentValue);
                return _currentValue.ToString();
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

    }
}