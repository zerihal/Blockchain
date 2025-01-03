using BlockchainUtils.Blockchains;

namespace BlockchainTestApp.Interfaces
{
    /// <summary>
    /// Blockchain runtime test interface.
    /// </summary>
    public interface IRunTest
    {
        /// <summary>
        /// Test name.
        /// </summary>
        public string RunTestName { get; }

        /// <summary>
        /// Test harness action that should be taken once test has been run.
        /// </summary>
        public RunAction RunAction { get; }

        /// <summary>
        /// Blockchain implementation for the test.
        /// </summary>
        BlockchainBase? RunTestBlockchain { get; }

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="args">Test arguments.</param>
        public void Run(object[] args);
    }
}
