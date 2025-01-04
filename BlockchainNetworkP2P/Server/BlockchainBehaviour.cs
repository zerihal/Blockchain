using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockchainNetworkP2P.Server
{
    // *** THIS CLASS IS TO BE DELETED - MOVED JUST TO P2PSERVER FOR SIMPLICITY ***
    public class BlockchainBehaviour : WebSocketBehavior
    {
        bool chainSynched = false;

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

                    if (!chainSynched)
                    {
                        Send(MessageHelper.SerializeMessage(Sandbox.SampleTransactionBlockchain));
                        chainSynched = true;
                    }

                    break;
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
