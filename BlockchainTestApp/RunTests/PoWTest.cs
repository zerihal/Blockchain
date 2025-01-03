using BlockchainUtils.Blockchains;
using BlockchainUtils.Blocks;
using Newtonsoft.Json;

namespace BlockchainTestApp.RunTests
{
    /// <summary>
    /// Proof of Work blockchain test.
    /// </summary>
    public class PoWTest : RunTestBase
    {
        public override string RunTestName => "PoW Test";

        public override void Run(object[] args)
        {
            var startTime = DateTime.Now;

            RunTestBlockchain = new PoWBlockchain();

            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine($"Adding Block {i}");
                RunTestBlockchain.AddBlock(GenerateSampleTemperatureBlock(i));
            }

            Console.WriteLine(JsonConvert.SerializeObject(RunTestBlockchain, Formatting.Indented));

            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");
        }

        private PoWBlock GenerateSampleTemperatureBlock(int temperature)
        {
            var blockData = string.Concat("{sender:Device1,receiver:CentralDevice,temperature:", temperature.ToString(), "}");
            return new PoWBlock(DateTime.Now, null, blockData);
        }
    }
}
