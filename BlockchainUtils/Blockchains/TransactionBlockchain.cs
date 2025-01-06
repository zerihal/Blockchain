using BlockchainUtils.Blocks;
using BlockchainUtils.Transactions;

namespace BlockchainUtils.Blockchains
{
    public class TransactionBlockchain : BlockchainBase
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();

        public int Reward { get; set; } = 1;

        public bool Initialised { get; private set; } = false;

        public TransactionBlockchain(bool init) : base() 
        {
            if (!init)
                Chain.Clear();

            Initialised = init;
        }

        public TransactionBlockchain()
        {
            Chain = new List<IBlock>();
        }

        public void CreateTransaction(Transaction transaction) => PendingTransactions.Add(transaction);

        /// <summary>
        /// Processes the latest pending transaction, adding a new transaction block as a reward for the miner who
        /// processed the transaction (which will be updated in their balance after the newly added transaction block
        /// has subsequently been processed). If there are no pending transactions or a miner processes a pending
        /// transaction that is there own only, no reward will be given for the processing (however if processing a
        /// pending transaction that is purely a miner's own processing transaction, it will still update the blockchain
        /// and therefore balance but just not create a new transaction for a reward).
        /// </summary>
        /// <param name="minerAddress">Miner address for the miner who has performed the processing.</param>
        /// <returns>
        /// True if pending transactions were processed, or false if there were no pending transactions to process
        /// or the miner who did the processing did this only on transactions relating to their own previous processing
        /// (thereby preventing them from gaining rewards for iteratively calling this method when there are no genuine
        /// transactions to process).
        /// </returns>
        public bool ProcessPendingTransactions(string minerAddress)
        {
            if (PendingTransactions.Count > 0)
            {
                var block = new TransactionBlock(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);

                // Check if all pending transactions relate just to this miner's processing of a previous block - if so
                // then they will still add the next block to the chain relating to their processing transaction, but will
                // not be permitted to create a new transaction for this (if there are other pending transactions too then
                // they will still create a new transaction for a reward for the processing though)
                var processOwnOnly = block.Transactions.All(t => t.FromAddress == null && t.ToAddress == minerAddress);
                AddBlock(block);

                PendingTransactions = new List<Transaction>();

                if (!processOwnOnly)
                {
                    CreateTransaction(new Transaction(null, minerAddress, Reward));
                    return true;
                }
            }

            return false;
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
                transBlock.Index = latestBlock.Index + 1;
                transBlock.PreviousHash = latestBlock.Hash;
                transBlock.Mine(Difficulty);
                Chain.Add(transBlock);
            }
        }
    }
}
