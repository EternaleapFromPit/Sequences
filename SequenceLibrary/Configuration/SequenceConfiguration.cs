using System.Configuration;

namespace SequenceLibrary.Configuration
{
    public class SequenceConfiguration : ISequenceConfiguration
    {
        public SequenceConfiguration()
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = "SequenceLibrary.dll.config";
            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

            SequenceNaturalNumbers = config.AppSettings.Settings["sequenceNaturalNumbers"].Value;
            SequenceTemplate = config.AppSettings.Settings["sequenceTemplate"].Value;
            ConnectionString = config.AppSettings.Settings["connectionString"].Value;
        }

        public string SequenceNaturalNumbers { get; private set; }

        public string SequenceTemplate { get; private set; }

        public string ConnectionString { get; private set; }
    }
}