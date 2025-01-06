using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using WebSocketSharp;

namespace BlockchainNetworkP2P.Client
{
    public class P2PClient
    {
        private IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();
        private BlockchainUtils utils = new BlockchainUtils();

        /// <summary>
        /// Client name.
        /// </summary>
        public string ClientName { get; }

        /// <summary>
        /// Number of messages processed by this client.
        /// </summary>
        public int MessagesProcessed { get; set; }

        public P2PClient(string clientName)
        {
            ClientName = clientName;
        }

        /// <summary>
        /// Connects to the P2P server at the specified URL, adding its websocket to the client cache, and
        /// hooks up events for that websocket. Optionally synchronizes the blockchain with the server.
        /// </summary>
        /// <param name="url">Server URL.</param>
        /// <param name="syncBlockchain">Optionally sync blockchain with the server (default is true).</param>
        public void Connect(string url, bool syncBlockchain = true)
        {
            if (!wsDict.ContainsKey(url))
            {
                WebSocket ws = new WebSocket(url);

                ws.OnMessage += (sender, e) =>
                {
                    var deserializedMsg = MessageHelper.DeserializeMessage(e.Data, out _);

                    switch (deserializedMsg)
                    {
                        case string str:
                            Console.WriteLine($"{ClientName} received message {str}");
                            
                            if (str == MessageHelper.ClientHello)
                                ws.Send(MessageHelper.SerializeMessage(Sandbox.SampleTransactionBlockchain));
                            
                            if (str == MessageHelper.NewPendingTransactions)
                                Sandbox.SampleTransactionBlockchain.ProcessPendingTransactions(ClientName);

                            break;

                        case TransactionBlockchain tBlockchain:
                            utils.UpdateSampleBlockchain(tBlockchain);
                            Console.WriteLine($"Client synced");
                            break;
                    }

                    MessagesProcessed++;
                };

                ws.OnOpen += (sender, e) =>
                {
                    Console.WriteLine($"{ClientName} connected to {url}");

                    if (syncBlockchain)
                        ws.Send(MessageHelper.SerializeMessage(MessageHelper.ServerHello));
                };

                ws.OnError += (sender, e) =>
                {
                    Console.WriteLine($"WebSocket error ({ClientName}): {e.Message}");
                };

                ws.OnClose += (sender, e) =>
                {
                    Console.WriteLine($"Connection closed ({ClientName}): {e.Reason}");
                };

                try
                {
                    ws.Connect();

                    // Add WebSocket to dictionary after successful connection
                    if (ws.ReadyState == WebSocketState.Open)
                        wsDict.Add(url, ws);
                    else
                        Console.WriteLine("Failed to open WebSocket connection.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect to {url}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"WebSocket already connected to {url}");
            }
        }

        /// <summary>
        /// Adds a transaction from this client and sends to the P2P server for processing.
        /// </summary>
        /// <param name="toAddress">Recipient of the transaction value.</param>
        /// <param name="amount">Amount of the transaction.</param>
        public void AddTransaction(string toAddress, int amount)
        {
            var transaction = new Transaction(ClientName, toAddress, amount);
            Send(wsDict.First().Key, MessageHelper.SerializeMessage(transaction));
        }

        /// <summary>
        /// Sends some data (string) to the specified URL.
        /// </summary>
        /// <param name="url">URL to send data to.</param>
        /// <param name="data">Data to send (string or JSON string for object).</param>
        public void Send(string url, string data)
        {
            foreach (var item in wsDict)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        /// <summary>
        /// Broadcasts data to all cached websocket endpoints.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        /// <summary>
        /// Gets a list of all cached P2P servers.
        /// </summary>
        /// <returns>Servers that have been connected to.</returns>
        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        /// <summary>
        /// Closes all connections and clears the cached websockets.
        /// </summary>
        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }

            wsDict.Clear();
        }
    }
}
