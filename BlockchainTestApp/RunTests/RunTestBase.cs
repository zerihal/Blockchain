using BlockchainTestApp.Interfaces;
using BlockchainUtils.Blockchains;

namespace BlockchainTestApp.RunTests
{
    public abstract class RunTestBase : IRunTest
    {
        public abstract string RunTestName { get; }

        public virtual RunAction RunAction => RunAction.ContinueShowMenu;

        public BlockchainBase? RunTestBlockchain { get; protected set; }

        public abstract void Run(object[] args);
    }
}
