using BlockchainNetworkP2P;
using BlockchainNetworkP2P.Client;
using BlockchainNetworkP2P.Server;

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

        public override string RunTestName => "P2P Transaction Sync Test";

        public override void Run(object[] args)
        {
            Console.WriteLine("Creating P2P server and clients");

            _server = new P2PServer();
            _clients = new List<P2PClient>() { new P2PClient("Client1") };

            Console.WriteLine("Running blockchain synchronisation test");

            // Start the server
            _server.Start(8080);

            // Reset processed message count before starting the test monitor (4 messages will be processed for this test to be completed)
            Sandbox.P2PMessagesProcessedCount = 0;
            _ = RunP2PTest();
            TestMonitor(4).Wait();

            Console.WriteLine("Testing P2P broadcast with 2 clients");

            // Reset processed message count before starting the test monitor (2 messages will be processed for this test to be completed)
            Sandbox.P2PMessagesProcessedCount = 0;
            _ = RunBroadcastTest();
            TestMonitor(2).Wait();

            // ToDo - test to create and process some transactions between the clients / server ...

            // Cleanup - stop the server, which will close the client connections
            _server.Stop();

            Console.WriteLine("\n");
        }

        private async Task RunP2PTest()
        {
            _clients.First().Connect(_server.SocketServiceAddress);

            await Task.CompletedTask;
        }

        private async Task RunBroadcastTest()
        {
            // Create and connect another client, but do not send sync messages
            _clients.Add(new P2PClient("Client2"));
            _clients[1].Connect(_server.SocketServiceAddress, false);
            _server.BroadcastTestMessage();
            await Task.CompletedTask;
        }

        private async Task TestMonitor(int awaitMsgCount)
        {
            while (Sandbox.P2PMessagesProcessedCount < awaitMsgCount)
                await Task.Delay(10);
        }
    }
}
