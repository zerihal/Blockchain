using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;

namespace BlockchainNetworkP2P
{
    internal class BlockchainUtils
    {
        internal void UpdateSampleBlockchain(TransactionBlockchain tBlockchain)
        {
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
        }
    }
}
