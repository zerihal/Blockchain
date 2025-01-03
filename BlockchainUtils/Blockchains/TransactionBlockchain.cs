using BlockchainUtils.Blocks;
using BlockchainUtils.Transactions;
using Newtonsoft.Json;

namespace BlockchainUtils.Blockchains
{
    public class TransactionBlockchain : BlockchainBase
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();

        public int Reward { get; set; } = 1;

        public TransactionBlockchain(bool init) : base() { }

        public TransactionBlockchain()
        {
            Chain = new List<IBlock>();
        }

        public void CreateTransaction(Transaction transaction) => PendingTransactions.Add(transaction);

        public void ProcessPendingTransactions(string minerAddress)
        {
            var block = new TransactionBlock(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }

        public int GetBalance(string minerAddress)
        {
            var balance = 0;

            foreach (var block in Chain)
            {
                if (block is ITransactionBlock tBlock)
                {
                    foreach (var trans in tBlock.Transactions)
                    {
                        if (trans.ToAddress == minerAddress)
                            balance += trans.Amount;
                    }
                }
            }

            return balance;
        }

        public override IBlock CreateGenesisBlock() => new TransactionBlock(DateTime.Now, null, PendingTransactions);

        public override void AddBlock(IBlock block)
        {
            TransactionBlock? latestBlock = GetLatestBlock() as TransactionBlock;

            if (latestBlock != null && block is TransactionBlock transBlock)
            {
                block.Index = latestBlock.Index + 1;
                block.PreviousHash = latestBlock.Hash;
                transBlock.Mine(Difficulty);
                Chain.Add(block);
            }
        }
    }
}
