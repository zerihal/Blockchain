using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainNetworkP2P.Server
{
    public class P2PServer : WebSocketBehavior
    {
        private const string _address = "ws://localhost";
        private const string _socketService = "/Blockchain";
        private WebSocketServer? _wss = null;
        private bool _chainSynced = false;

        public string ServerAddress { get; private set; } = string.Empty;

        public string SocketServiceAddress { get; private set; } = string.Empty;

        public void Start(int port)
        {
            ServerAddress = $"{_address}:{port}";
            SocketServiceAddress = $"{ServerAddress}{_socketService}";

            _wss = new WebSocketServer(ServerAddress);
            _wss.AddWebSocketService<P2PServer>(_socketService);
            _wss.Start();
            Console.WriteLine($"Started server at {ServerAddress}");
        }

        /// <summary>
        /// Stop the server and all incoming requests and closes all connections.
        /// </summary>
        public void Stop() => _wss?.Stop(CloseStatusCode.Normal, "Server stopped");

        /// <summary>
        /// Broadcast a test message to all clients.
        /// </summary>
        public void BroadcastTestMessage()
        {
            if (_wss != null)
            {
                foreach (var host in _wss.WebSocketServices.Hosts)
                    host.Sessions.Broadcast(MessageHelper.SerializeMessage(MessageHelper.Test));
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var deserializedMsg = MessageHelper.DeserializeMessage(e.Data, out var msgJsonData);

            Console.WriteLine($"Server received message: {msgJsonData}");

            switch (deserializedMsg)
            {
                case string str:
                    if (str == MessageHelper.ServerHello)
                        Send(MessageHelper.SerializeMessage(MessageHelper.ClientHello));
                    break;

                case TransactionBlockchain tBlockchain:
                    // If the blockchain that has been received is valid and has more blocks than the sample one, update
                    // the sample blockchain.
                    if (tBlockchain != null && BlockchainHelper.IsValidBlockchain(tBlockchain, out _) &&
                        tBlockchain.Chain.Count > Sandbox.SampleTransactionBlockchain.Chain.Count)
                    {
                        var newTransactions = new List<Transaction>();
                        newTransactions.AddRange(tBlockchain.PendingTransactions);
                        newTransactions.AddRange(Sandbox.SampleTransactionBlockchain.PendingTransactions);

                        tBlockchain.PendingTransactions = newTransactions;
                        Sandbox.SampleTransactionBlockchain = tBlockchain;
                    }

                    if (!_chainSynced)
                    {
                        Send(MessageHelper.SerializeMessage(Sandbox.SampleTransactionBlockchain));
                        _chainSynced = true;
                        Console.WriteLine($"Blockchain synchronised for session {ID}");
                    }

                    break;
            }

            Sandbox.P2PMessagesProcessedCount++;
        }

        protected override void OnOpen()
        {
            Console.WriteLine("Websocket session opened");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Websocket session closed");
        }
    }
}
