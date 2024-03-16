using BlockchainUtils.Blockchains;
using BlockchainUtils.Blocks;
using Newtonsoft.Json;

namespace BlockchainTestApp.RunTests
{
    public class BasicHashTest : RunTestBase
    {
        public override string RunTestName => "Basic Hash Test";

        public override void Run(object[] args)
        {
            var startTime = DateTime.Now;

            RunTestBlockchain = new BasicBlockchain();
            
            for (int i = 0; i <= 5; i++) 
            {
                Console.WriteLine($"Adding Block {i}");
                RunTestBlockchain.AddBlock(GenerateSampleTemperatureBlock(i));
            }

            Console.WriteLine(JsonConvert.SerializeObject(RunTestBlockchain, Formatting.Indented));

            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");
        }

        private BasicBlock GenerateSampleTemperatureBlock(int temperature)
        {
            var blockData = string.Concat("{sender:Device1,receiver:CentralDevice,temperature:", temperature.ToString(), "}");
            return new BasicBlock(DateTime.Now, null, blockData);
        }
    }
}
