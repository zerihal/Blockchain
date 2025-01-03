using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using Newtonsoft.Json;

namespace BlockchainTestApp.RunTests
{
    /// <summary>
    /// This test simulates simple transactions - the blocks that are created and added to the
    /// chain contain a sender and receiver for the transaction with the amount (say in some sort
    /// of cryptocurrency for example), however the user that processes the block (i.e. completes
    /// the PoW) also gets a reward from doing the work, which is then included in the Blockchain.
    /// In reality of course, there would need to be verification and such like before a reward
    /// would be given (i.e. block would need to be valid and not tampered with) and blocks could
    /// probably contain data other than a simple integer for the amount, but this example is just
    /// to show a basic PoW and reward mechanism.
    /// </summary>
    public class TransactionTest : RunTestBase
    {
        public override string RunTestName => "Transaction Test";

        public override void Run(object[] args)
        {
            var startTime = DateTime.Now;

            TransactionBlockchain RunTestBlockchain = new TransactionBlockchain(true);
            RunTestBlockchain.CreateTransaction(new Transaction("ZS", "NN", 10));
            RunTestBlockchain.ProcessPendingTransactions("JD");

            // Reward is added after block was processed (see last line in the
            // TransactionBlockchain.ProcessPendingTransactions method - this adds a new transaction
            // block to the chain for the reward for the user, which is then in the pending transactions
            // list to be processed), so it will not show in the output from the below
            Console.WriteLine(JsonConvert.SerializeObject(RunTestBlockchain, Formatting.Indented));

            RunTestBlockchain.CreateTransaction(new Transaction("ZS", "NN", 5));
            RunTestBlockchain.CreateTransaction(new Transaction("ZS", "NN", 5));
            RunTestBlockchain.ProcessPendingTransactions("JD");

            var endTime = DateTime.Now;
            Console.WriteLine($"Duration: {endTime - startTime}");

            // We now should see the reward for "JD" in the Blockchain as pending transactions have
            // now been processed (although as JD did this work/processing for this there will be 
            // another reward in pending transactions for that user)
            Console.WriteLine(JsonConvert.SerializeObject(RunTestBlockchain, Formatting.Indented));

            // Output the balances
            Console.WriteLine($"Balance for JD: {RunTestBlockchain.GetBalance("JD")}");
            Console.WriteLine($"Balance for ZS: {RunTestBlockchain.GetBalance("ZS")}");
            Console.WriteLine($"Balance for NN: {RunTestBlockchain.GetBalance("NN")}\n");
        }
    }
}
