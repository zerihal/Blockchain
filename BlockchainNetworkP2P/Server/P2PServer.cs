using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainNetworkP2P.Server
{
    public class P2PServer : WebSocketBehavior
    {
        private const string _address = "ws://localhost";
        private const string _socketService = "/Blockchain";
        private static WebSocketServer? _wss;
        private bool _chainSynced = false;
        private BlockchainUtils _utils = new BlockchainUtils();
        private BackgroundWorker _worker = new BackgroundWorker() { WorkerSupportsCancellation = true };
        private ObservableCollection<Transaction> _pendingTransactions = new ObservableCollection<Transaction>();

        public string ServerAddress { get; private set; } = string.Empty;

        public string SocketServiceAddress { get; private set; } = string.Empty;

        public P2PServer()
        {
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerAsync();
        }

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
        public void Stop()
        {
            _wss?.Stop(CloseStatusCode.Normal, "Server stopped");
            _wss = null;
        }

        /// <summary>
        /// Broadcast a test message to all clients.
        /// </summary>
        public void BroadcastTestMessage() => BroadcastMessage(MessageHelper.SerializeMessage(MessageHelper.Test));

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
                    _utils.UpdateSampleBlockchain(tBlockchain);

                    if (!_chainSynced)
                    {
                        Send(MessageHelper.SerializeMessage(Sandbox.SampleTransactionBlockchain));
                        _chainSynced = true;
                        Console.WriteLine($"Blockchain synchronised for session {ID}");
                    }

                    break;

                case Transaction transaction:
                    Sandbox.SampleTransactionBlockchain.CreateTransaction(transaction);
                    _pendingTransactions.Add(transaction);
                    break;
            }

            // Needs to update a static variable for server messages due to potential for multiple sessions for this
            // instance of server.
            Sandbox.ServerMessagesProcessed++;
        }

        protected override void OnOpen()
        {
            Console.WriteLine("Websocket session opened");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("Websocket session closed");
        }

        private void BroadcastMessage(string message)
        {
            if (_wss != null)
            {
                foreach (var host in _wss.WebSocketServices.Hosts)
                    host.Sessions.Broadcast(message);
            }
        }

        private void _worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            _pendingTransactions.CollectionChanged += (object ? sender, NotifyCollectionChangedEventArgs e) => 
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    Sandbox.SampleTransactionBlockchain.ProcessPendingTransactions("Server");

                    if (e.NewItems?.Cast<Transaction>().FirstOrDefault() is Transaction transaction)
                        _pendingTransactions.Remove(transaction);

                    Send(MessageHelper.SerializeMessage("Transaction processed"));
                }
            };
        }
    }
}
