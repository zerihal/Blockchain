using BlockchainUtils.Blockchains;

namespace BlockchainTestApp.Interfaces
{
    public interface IRunTest
    {
        public string RunTestName { get; }

        public RunAction RunAction { get; }

        BlockchainBase? RunTestBlockchain { get; }

        public void Run(object[] args);
    }
}
