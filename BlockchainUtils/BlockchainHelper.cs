using BlockchainUtils.Blockchains;

namespace BlockchainUtils
{
    public static class BlockchainHelper
    {
        public static bool IsValidBlockchain(BlockchainBase? blockchain, out IList<int>invalidBlocks)
        {
            invalidBlocks = new List<int>();

            if (blockchain == null)
                throw new ArgumentNullException("Blockchain is null!");

            for (int i = 1; i < blockchain.Chain.Count; i++)
            {
                var currentBlock = blockchain.Chain[i];
                var previousBlock = blockchain.Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    invalidBlocks.Add(i);
                }
                else if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    invalidBlocks.Add(i);
                }
            }

            return invalidBlocks.Count == 0;
        }
    }
}
