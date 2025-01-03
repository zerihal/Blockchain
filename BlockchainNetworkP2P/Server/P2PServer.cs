using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainNetworkP2P.Server
{
    public class P2PServer : WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer? wss = null;

        public void Start(int port)
        {
            // ToDo: To test this we need to create an instance of client and server, start, and then add some transactions, etc

            wss = new WebSocketServer($"ws://127.0.0.1:{port}");
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{port}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                TransactionBlockchain? newChain = JsonConvert.DeserializeObject<TransactionBlockchain>(e.Data);

                if (newChain != null && BlockchainHelper.IsValidBlockchain(newChain, out _) && 
                    newChain.Chain.Count > Sandbox.SampleTransactionBlockchain.Chain.Count)
                {
                    var newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Sandbox.SampleTransactionBlockchain.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Sandbox.SampleTransactionBlockchain = newChain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Sandbox.SampleTransactionBlockchain));
                    chainSynched = true;
                }
            }
        }
    }
}
