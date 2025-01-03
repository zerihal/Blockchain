using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainNetworkP2P.Server
{
    public class BlockchainBehaviour : WebSocketBehavior
    {
        bool chainSynched = false;

        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine($"Server received message: {e.Data}");
            if (e.Data == Sandbox.ServerHello)
            {
                Console.WriteLine(e.Data);
                Send(Sandbox.ClientHello);
            }
            else
            {
                TransactionBlockchain? newChain = JsonConvert.DeserializeObject<TransactionBlockchain>(e.Data, Sandbox.JsonSettings);

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
                    Send(JsonConvert.SerializeObject(Sandbox.SampleTransactionBlockchain, Sandbox.JsonSettings));
                    chainSynched = true;
                }
            }
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
