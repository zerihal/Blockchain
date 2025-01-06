using BlockchainUtils;
using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;

namespace BlockchainNetworkP2P
{
    internal class BlockchainUtils
    {
        /// <summary>
        /// Updates the sample blockchain with the one specified if more recent (chain is longer).
        /// </summary>
        /// <param name="tBlockchain">Blockchain to use to potentially update sample blockchain.</param>
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
