using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using WebSocketSharp;

namespace BlockchainNetworkP2P.Client
{
    /// <summary>
    /// See https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-p2p-network/ for details.
    /// </summary>
    public class P2PClient
    {
        private IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

        public string ClientName { get; }

        public P2PClient(string clientName)
        {
            ClientName = clientName;
        }

        public void Connect(string url)
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
                            Console.WriteLine($"{ClientName} received messasge {str}");
                            if (str == MessageHelper.ClientHello)
                                ws.Send(MessageHelper.SerializeMessage(Sandbox.SampleTransactionBlockchain));
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
                            break;
                    }
                };

                ws.OnOpen += (sender, e) =>
                {
                    Console.WriteLine($"{ClientName} connected to {url}");
                    ws.Send(MessageHelper.SerializeMessage(MessageHelper.ServerHello));
                };

                ws.OnError += (sender, e) =>
                {
                    Console.WriteLine($"WebSocket error: {e.Message}");
                };

                ws.OnClose += (sender, e) =>
                {
                    Console.WriteLine($"Connection closed: {e.Reason}");
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

        // ToDo - need to add some bits to this test - could send transaction to the server (would need to handle on message received), 
        // which could then broadcast that there are pending transactions that can be processed maybe?
        public void AddTransaction(string toAddress, int amount)
        {
            var transaction = new Transaction(ClientName, toAddress, amount);
            Send(wsDict.First().Key, MessageHelper.SerializeMessage(transaction));
        }

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

        public void Broadcast(string data)
        {
            foreach (var item in wsDict)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in wsDict)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in wsDict)
            {
                item.Value.Close();
            }
        }
    }
}
