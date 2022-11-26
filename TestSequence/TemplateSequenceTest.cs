namespace TestSequence
{
    using Moq;
    using SequenceLibrary.Configuration;
    using SequenceLibrary.Repository;
    using SequenceLibrary.Sequences;

    public class TemplateSequenceTest
    {
        private Mock<ISequenceRepository> mockRepo = new Mock<ISequenceRepository>();

        [Fact]
        public void Verify_config_loaded_and_correct()
        {
            var typedCfg = new SequenceConfiguration();
            Assert.NotNull(typedCfg.SequenceTemplate);
        }

        [Fact]
        public void Verify_sequence_parsed()
        {
            var templateSequence = new TemplateSequence("[ABC][2017][0..1]", mockRepo.Object);
            Assert.Equal("ABC20170", templateSequence.GetCurrent());
            mockRepo.Verify(x => x.Update(TemplateSequence.Name, It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void Verify_sequence_cycled()
        {
            const int end = 33;

            var templateSequence = new TemplateSequence($"[ABC][2017][0..{end}]", mockRepo.Object);

            for (int i = 1; i <= end; i++)
            {
                Assert.Equal($"ABC2017{i}", templateSequence.GetNext().Result);
            }
            Assert.Equal($"ABC20170", templateSequence.GetNext().Result);
            mockRepo.Verify(x => x.Update(TemplateSequence.Name, It.IsAny<int>()), Times.Exactly(end+1));
        }
    }
}