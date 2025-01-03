using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using Newtonsoft.Json;
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
                    if (e.Data == Sandbox.ClientHello)
                    {
                        Console.WriteLine($"{ClientName} received messasge {e.Data}");
                        ws.Send(JsonConvert.SerializeObject(Sandbox.SampleTransactionBlockchain, Sandbox.JsonSettings));
                    }
                    else if (e.Data == Sandbox.Test)
                    {
                        Console.WriteLine($"{ClientName} received messasge {e.Data}");
                    }
                    else
                    {
                        var newChain = JsonConvert.DeserializeObject<TransactionBlockchain>(e.Data, Sandbox.JsonSettings);

                        // If the blockchain that has been received is valid and has more blocks than the sample one, update
                        // the sample blockchain.
                        if (newChain != null && BlockchainHelper.IsValidBlockchain(newChain, out _) &&
                            newChain.Chain.Count > Sandbox.SampleTransactionBlockchain.Chain.Count)
                        {
                            var newTransactions = new List<Transaction>();
                            newTransactions.AddRange(newChain.PendingTransactions);
                            newTransactions.AddRange(Sandbox.SampleTransactionBlockchain.PendingTransactions);

                            newChain.PendingTransactions = newTransactions;
                            Sandbox.SampleTransactionBlockchain = newChain;
                        }
                    }
                };

                ws.OnOpen += (sender, e) =>
                {
                    Console.WriteLine($"{ClientName} connected to {url}");
                    ws.Send(Sandbox.ServerHello);
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

        public void AddTransaction(string toAddress, int amount)
        {
            var transaction = new Transaction(ClientName, toAddress, amount);
            Send(wsDict.First().Key, JsonConvert.SerializeObject(transaction));
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
