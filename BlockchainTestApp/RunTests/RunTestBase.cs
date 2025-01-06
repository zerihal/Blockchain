using BlockchainTestApp.Interfaces;
using BlockchainUtils.Blockchains;

namespace BlockchainTestApp.RunTests
{
    public abstract class RunTestBase : IRunTest
    {
        /// <inheritdoc/>
        public abstract string RunTestName { get; }

        /// <inheritdoc/>
        public virtual RunAction RunAction => RunAction.ContinueShowMenu;

        /// <inheritdoc/>
        public BlockchainBase? RunTestBlockchain { get; protected set; }

        /// <inheritdoc/>
        public abstract void Run(object[]? args);
    }
}
