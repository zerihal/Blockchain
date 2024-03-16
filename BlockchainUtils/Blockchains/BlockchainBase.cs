using BlockchainUtils.Blocks;

namespace BlockchainUtils.Blockchains
{
    public abstract class BlockchainBase
    {
        public IList<IBlock> Chain { set; get; }

        /// <summary>
        /// The difficulty is an integer that indicates the number of leading zeros required for a generated hash - the
        /// higher the number the more difficult it will be to mine (will require more nonce increments to find a hash)
        /// </summary>
        public int Difficulty { set; get; } = 3;

        protected BlockchainBase()
        {
            Chain = new List<IBlock>();
            AddGenesisBlock();
        }

        public abstract IBlock CreateGenesisBlock();

        public void AddGenesisBlock() => Chain.Add(CreateGenesisBlock());

        public IBlock GetLatestBlock() => Chain[Chain.Count - 1];

        public abstract void AddBlock(IBlock block);
    }
}
