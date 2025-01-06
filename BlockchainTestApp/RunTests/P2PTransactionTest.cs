using BlockchainNetworkP2P;
using BlockchainNetworkP2P.Client;
using BlockchainNetworkP2P.Server;
using BlockchainUtils.Blockchains;

namespace BlockchainTestApp.RunTests
{
    /// <summary>
    /// Blockchain P2P transaction test - this test uses a transaction blockchain between P2P clients and server, in addition to
    /// testing the simple P2P network functionality itself. Due to the async nature of the P2P messages, in order to prevent the
    /// completion of the test before all test messages have been processed, the functionality has been put into async tasks with
    /// a monitor to impose an async await until all expected messages have been processed for each test segment.
    /// </summary>
    public class P2PTransactionTest : RunTestBase
    {
        #pragma warning disable CS8618
        private P2PServer _server;
        private IList<P2PClient> _clients;
        #pragma warning restore CS8618

        /// <inheritdoc/>
        public override string RunTestName => "P2P Transaction Sync Test";

        /// <inheritdoc/>
        public override void Run(object[]? args)
        {
            Console.WriteLine("Creating P2P server and clients");

            _server = new P2PServer();
            _clients = new List<P2PClient>() { new P2PClient("Client1"), new P2PClient("Client2") };

            Console.WriteLine("Running blockchain synchronization test with 2 transactions from Client1 to Client2 with Server to process");

            // Start the server
            _server.Start(8080);

            // Reset processed message count before starting the test monitor (12 messages will be processed for this test to be completed)
            ResetProcessedMessages();
            _ = RunP2PTest();
            TestMonitor(12).Wait();

            Console.WriteLine("Testing P2P broadcast with 2 clients");

            // Reset processed message count before starting the test monitor (2 messages will be processed for this test to be completed)
            ResetProcessedMessages();
            _ = RunBroadcastTest();
            TestMonitor(2).Wait();

            // Update the balance of the server for the transactions that it has processed
            Sandbox.SampleTransactionBlockchain.ProcessPendingTransactions("Server");

            Console.WriteLine($"Balance for Client1: {Sandbox.SampleTransactionBlockchain.GetBalance("Client1")}");
            Console.WriteLine($"Balance for Client2: {Sandbox.SampleTransactionBlockchain.GetBalance("Client2")}");
            Console.WriteLine($"Balance for Server: {Sandbox.SampleTransactionBlockchain.GetBalance("Server")}");

            // Cleanup - stop the server, which will close the client connections, and reset sample blockchain
            Cleanup();

            Console.WriteLine("\n");
        }

        /// <summary>
        /// Runs P2P test as an async task.
        /// </summary>
        private async Task RunP2PTest()
        {
            // Connect and sync the P2P clients and server
            _clients[0].Connect(_server.SocketServiceAddress);
            _clients[1].Connect(_server.SocketServiceAddress);

            // Wait a couple of seconds for clients and server to sync so console output is in a tidy order
            await Task.Delay(2000);

            // Send a couple of transactions from Client1 to Client2
            _clients.First().AddTransaction("Client2", 5);
            _clients.First().AddTransaction("Client2", 10);

            await Task.CompletedTask;
        }

        /// <summary>
        /// Runs P2P broadcast test as an async task.
        /// </summary>
        private async Task RunBroadcastTest()
        {
            _server.BroadcastTestMessage();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Runs a test monitor that can be awaited until the expected number of P2P messages have been 
        /// processed.
        /// </summary>
        /// <param name="awaitMsgCount">Expected message count that test can wait for.</param>
        private async Task TestMonitor(int awaitMsgCount)
        {
            while (GetProcessedMessages() < awaitMsgCount)
                await Task.Delay(10);
        }

        /// <summary>
        /// Gets the total number of processed messages from the P2P server and all clients.
        /// </summary>
        /// <returns>Processed message count.</returns>
        private int GetProcessedMessages()
        {
            var processedMessages = Sandbox.ServerMessagesProcessed;

            foreach (var client in _clients)
                processedMessages += client.MessagesProcessed;

            return processedMessages;
        }

        /// <summary>
        /// Resets the processed message count to 0 for the P2P server and all clients.
        /// </summary>
        private void ResetProcessedMessages()
        {
            Sandbox.ServerMessagesProcessed = 0;

            foreach (var client in _clients)
                client.MessagesProcessed = 0;
        }

        /// <summary>
        /// Test cleanup - stop the server and reset the sample transaction blockchain.
        /// </summary>
        private void Cleanup()
        {
            _server.Stop();
            Sandbox.SampleTransactionBlockchain = new TransactionBlockchain(true);
        }
    }
}
