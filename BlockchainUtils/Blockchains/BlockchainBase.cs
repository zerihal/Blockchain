using BlockchainUtils.Blocks;

namespace BlockchainUtils.Blockchains
{
    public abstract class BlockchainBase
    {
        /// <summary>
        /// The blockchain (list of all contained, ordered, and hash linked blocks).
        /// </summary>
        public IList<IBlock> Chain { set; get; }

        /// <summary>
        /// The mining difficulty.
        /// </summary>
        /// <remarks>
        /// The difficulty is an integer that indicates the number of leading zeros required for a generated hash - the
        /// higher the number the more difficult it will be to mine (will require more nonce increments to find a hash).
        /// </remarks>
        public int Difficulty => BlockchainSettings.MineDifficulty;

        protected BlockchainBase()
        {
            Chain = new List<IBlock>();
            AddGenesisBlock();
        }

        /// <summary>
        /// Create the "Genesis" block, which is the first block in the chain. This will be the only block
        /// in the chain that has no previous hash and will likely have no (or empty) contained data.
        /// </summary>
        /// <returns>Genesis block to initialize the blockchain.</returns>
        public abstract IBlock CreateGenesisBlock();

        /// <summary>
        /// Creates and adds a genesis (initial) block to the chain to initialize it.
        /// </summary>
        public void AddGenesisBlock() => Chain.Add(CreateGenesisBlock());

        /// <summary>
        /// Gets the latest (last added) block in the blockchain.
        /// </summary>
        /// <returns>Latest block in the chain.</returns>
        public IBlock GetLatestBlock() => Chain[Chain.Count - 1];

        /// <summary>
        /// Adds a block to the blockchain.
        /// </summary>
        /// <param name="block">Block to add to the blockchain, hash linked to the previous block.</param>
        public abstract void AddBlock(IBlock block);
    }
}
