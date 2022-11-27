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
        public void Verify_repository_create_update_called()
        {
            const int start = 1;
            const int end = 2;

            var sequence = new TemplateSequence($"[ABC][2017][{start}..{end}]", mockRepo.Object);

            for (int i = (start + 1); i <= end; i++)
            {
                Assert.Equal($"ABC2017{i}".ToString(), sequence.GetNext().Result);
            }

            mockRepo.Verify(x => x.Create(TemplateSequence.Name, It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Update(TemplateSequence.Name, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Verify_sequence_cycled()
        {
            const int start = 1;
            const int end = 33;

            var templateSequence = new TemplateSequence($"[ABC][2017][{start}..{end}]", mockRepo.Object);

            for (int i = (start + 1); i <= end; i++)
            {
                Assert.Equal($"ABC2017{i}", templateSequence.GetNext().Result);
            }

            Assert.Equal($"ABC2017{start}", templateSequence.GetNext().Result);
            mockRepo.Verify(x => x.Update(TemplateSequence.Name, It.IsAny<int>()), Times.Exactly(end));
        }
    }
}