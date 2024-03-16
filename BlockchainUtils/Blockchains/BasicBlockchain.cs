using BlockchainUtils.Blocks;

namespace BlockchainUtils.Blockchains
{
    /// <summary>
    /// Very simple Blockchain class using hashed blocks.
    /// See https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/ for details.
    /// </summary>
    public class BasicBlockchain : BlockchainBase
    {
        public BasicBlockchain() : base() { }

        public override IBlock CreateGenesisBlock() => new BasicBlock(DateTime.Now, null, "{}");

        public override void AddBlock(IBlock block)
        {
            BasicBlock? latestBlock = GetLatestBlock() as BasicBlock;

            if (latestBlock != null)
            {
                block.Index = latestBlock.Index + 1;
                block.PreviousHash = latestBlock.Hash;
                block.Hash = block.CalculateHash();
                Chain.Add(block);
            }
        }
    }
}
