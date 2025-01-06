using BlockchainUtils.Transactions;

namespace BlockchainUtils.Blocks
{
    public interface ITransactionBlock
    {
        /// <summary>
        /// List of transactions within this block.
        /// </summary>
        public IList<Transaction> Transactions { get; set; }
    }
}
