using BlockchainUtils.Blockchains;

namespace BlockchainUtils
{
    public static class BlockchainHelper
    {
        /// <summary>
        /// Checks whether the blockchain is valid (blocks have not been modified or substituted).
        /// </summary>
        /// <param name="blockchain">The blockchain to validate.</param>
        /// <param name="invalidBlocks">List of invalid blocks (if any) by index.</param>
        /// <returns>True if the blockchain is valid (no tampering), otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if blockchain arg is null.</exception>
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
