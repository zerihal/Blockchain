using BlockchainUtils.Blocks;

namespace BlockchainUtils.Blockchains
{
    public abstract class BlockchainBase
    {
        public IList<IBlock> Chain { set; get; }

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
