namespace SequenceLibrary.Configuration
{
    /// <summary>
    /// Configuration for the sequences. 
    /// Could be implemented in a different ways. app.config inside of library itself could be overriden by service appsettings.json configuration or something else. 
    /// Library itself contains default implementation.
    /// </summary>
    public interface ISequenceConfiguration
    {
        public string SequenceNaturalNumbers { get; }

        public string SequenceTemplate { get; }

        public string SequenceCycleCondition { get; }

        public string ConnectionString { get; }

    }
}
