using BlockchainUtils.Blocks;

namespace BlockchainUtils.Blockchains
{
    public class PoWBlockchain : BlockchainBase
    {
        public PoWBlockchain() : base() { }

        public override IBlock CreateGenesisBlock() => new PoWBlock(DateTime.Now, null, "{}");

        public override void AddBlock(IBlock block)
        {
            PoWBlock? latestBlock = GetLatestBlock() as PoWBlock;

            if (latestBlock != null && block is PoWBlock powBlock)
            {
                block.Index = latestBlock.Index + 1;
                block.PreviousHash = latestBlock.Hash;
                powBlock.Mine(Difficulty);
                Chain.Add(block);
            }
        }
    }
}
