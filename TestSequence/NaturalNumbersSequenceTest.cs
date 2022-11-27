using Moq;
using SequenceLibrary.Configuration;
using SequenceLibrary.DTO;
using SequenceLibrary.Repository;
using SequenceLibrary.Sequences;

namespace TestSequence
{
    public class NaturalNumbersSequenceTest
    {
        private Mock<ISequenceRepository> mockRepo = new Mock<ISequenceRepository>();

        [Fact]
        public void Verify_config_loaded_and_correct()
        {
            var typedCfg = new SequenceConfiguration();

            Assert.NotNull(typedCfg.SequenceNaturalNumbers);
        }

        [Fact]
        public void Verify_repository_create_update_called()
        {
            const int start = 1;
            const int end = 2;

            var sequence = new NaturalNumbersSequence($"[{start}..{end}]", mockRepo.Object);

            for (int i = (start + 1); i <= end; i++)
            {
                Assert.Equal(i.ToString(), sequence.GetNext().Result);
            }

            mockRepo.Verify(x => x.Create(NaturalNumbersSequence.Name, It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            mockRepo.Verify(x => x.Update(NaturalNumbersSequence.Name, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Verify_sequence_cycled()
        {
            const int start = 5;
            const int end = 43;

            var sequence = new NaturalNumbersSequence($"[{start}..{end}]", mockRepo.Object);

            for (int i = (start + 1); i <= end; i++)
            {
                Assert.Equal(i.ToString(), sequence.GetNext().Result);
            }

            Assert.Equal(start.ToString(), sequence.GetNext().Result);
            mockRepo.Verify(x => x.Update(NaturalNumbersSequence.Name, It.IsAny<int>()), Times.Exactly(end - start + 1));
        }

        [Fact]
        public void Verify_sequence_renewed_after_year_change()
        {
            const int start = 5;
            const int end = 43;

            mockRepo.Setup(x => x.Read(It.IsAny<string>())).ReturnsAsync(new SequenceDto
            {
                Value = end,
                Year = "2021"
            });

            var sequence = new NaturalNumbersSequence($"[{start}..{end}]", mockRepo.Object);
            
            Assert.Equal("6", sequence.GetNext().Result);
            mockRepo.Verify(x => x.Read(NaturalNumbersSequence.Name), Times.Once);
        }
    }
}