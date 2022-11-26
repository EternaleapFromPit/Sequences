using SequenceLibrary.Configuration;
using SequenceLibrary.Repository;

namespace TestSequence
{
    public class TestDatabase
    {
        [Fact]
        public void Check_connection_established()
        {
            var typedCfg = new SequenceConfiguration();
            var ex = Record.Exception(() => new MsSqlSequenceRepository(new DapperContext(typedCfg)).Read("test").Result);
            Assert.Null(ex);
        }
    }
}
