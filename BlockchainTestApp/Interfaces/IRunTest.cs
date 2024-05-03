using BlockchainUtils.Blockchains;

namespace BlockchainTestApp.Interfaces
{
    public interface IRunTest
    {
        string RunTestName { get; }

        RunAction RunAction { get; }

        BlockchainBase? RunTestBlockchain { get; }

        string Description { get; }

        void Run(object[] args);
    }
}
