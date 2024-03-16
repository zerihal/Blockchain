using BlockchainUtils.Transactions;

namespace BlockchainUtils.Blocks
{
    public interface ITransactionBlock
    {
        public IList<Transaction> Transactions { get; set; }
    }
}
