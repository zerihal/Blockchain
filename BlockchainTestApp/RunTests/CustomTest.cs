namespace BlockchainTestApp.RunTests
{
    /// <summary>
    /// Custom test (not yet implemented).
    /// </summary>
    public class CustomTest : RunTestBase
    {
        /// <inheritdoc/>
        public override string RunTestName => "Custom Test";

        /// <inheritdoc/>
        public override void Run(object[]? args)
        {
            Console.WriteLine("This run test has not yet been implemented");
            Console.WriteLine("Please make an alternate selection");
        }
    }
}
