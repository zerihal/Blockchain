using BlockchainUtils.Blocks;

namespace BlockchainUtils.Blockchains
{
    public class PoWBlockchain : BlockchainBase
    {
        public PoWBlockchain() : base() { }

        /// <summary>
        /// The difficulty is an integer that indicates the number of leading zeros required for a generated hash - the
        /// higher the number the more difficult it will be to mine (will require more nonce increments to find a hash)
        /// </summary>
        public int Difficulty { set; get; } = 3;

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
