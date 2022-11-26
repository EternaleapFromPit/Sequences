using SequenceLibrary.Repository;

namespace SequenceLibrary.Sequences
{
    public class TemplateSequence : ISequence
    {
        private ISequenceRepository _repository;
        private string[] _templateElems;
        private string _item;
        private int _currentValue;
        private int _startValue;
        private int _endValue;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public const string Name = "Template";

        public TemplateSequence(string template, ISequenceRepository repository)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentException("template is empty");

            if (repository is null)
                throw new ArgumentException("repository instance for the sequence does not exist");

            _repository = repository;

            _templateElems = template.Split('[', ']').Where(x => x != "").ToArray();
            var rangeElems = _templateElems[2].Split('.').Where(x => x != "");
            _startValue = Convert.ToInt32(rangeElems.First());
            _endValue = Convert.ToInt32(rangeElems.Last());
            
            var year = _templateElems[1];

            var storedValue = repository.Read(Name)?.Result;
            _currentValue = storedValue?.Value ?? _startValue;

            if (storedValue?.Value == null)
                _repository.Create(Name, _currentValue, year);
            else if (year != storedValue.Year) //year had change hence restarting the sequence
                _currentValue = _startValue;
                

            _item = CreateNext();
        }

        public string GetCurrent() => _item;
        

        public async Task<string> GetNext()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                _currentValue++;
                await _repository.Update(Name, _currentValue);
                return CreateNext();
            }
            finally 
            {
                semaphoreSlim.Release();
            }
        }

        private string CreateNext()
        {
            if (_currentValue > _endValue)
                _currentValue = _startValue;
            _item = _templateElems[0] + _templateElems[1] + _currentValue;
            return _item;
        }
    }
}
