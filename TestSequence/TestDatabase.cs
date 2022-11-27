using Microsoft.Extensions.Logging;
using Moq;
using SequenceLibrary.Configuration;
using SequenceLibrary.Repository;

namespace TestSequence
{
    public class TestDatabase
    {
        [Fact(Skip = "ignored in github pipeline")]
        public async void Check_connection_established()
        {
            var typedCfg = new SequenceConfiguration();
            var ex = await Record.ExceptionAsync(async () => { await new MsSqlSequenceRepository(new DapperContext(typedCfg), new Mock<ILogger<MsSqlSequenceRepository>>().Object).Read("test"); });
            Assert.Null(ex);
        }
    }
}
