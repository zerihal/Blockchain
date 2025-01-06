using BlockchainUtils.Blockchains;

namespace BlockchainUtils.Validation
{
    public static class TamperValidate
    {
        /// <summary>
        /// Tampers with the blockchain by tampering with the data and hash, checking that validation fails
        /// and outputting actions / results to console.
        /// </summary>
        /// <param name="blockchain"></param>
        public static void Tamper(BlockchainBase blockchain)
        {
            var tamperBlock = GetRandomBlock(blockchain);

            Console.WriteLine("Tampering with Block data");
            blockchain.Chain[tamperBlock].Data = "{sender:Device1,receiver:CentralDevice,temperature:35}";

            // Blockchain should now be invalid - check this and then try to tamper with the hash (block should still be invalid)
            if (!BlockchainHelper.IsValidBlockchain(blockchain, out var invalidBlocks1))
            {
                Console.WriteLine("Blockchain invalid - invalid blocks:");

                foreach (var block in invalidBlocks1)
                    Console.WriteLine($"{block}");

                Console.WriteLine("Tampering with Block hash for invalid block");
                var otherBlock = GetRandomBlock(blockchain, tamperBlock);

                blockchain.Chain[tamperBlock].Hash = blockchain.Chain[otherBlock].CalculateHash();
                BlockchainHelper.IsValidBlockchain(blockchain, out var invalidBlocks2);

                Console.WriteLine("Blockchain now has the following invalid blocks:");

                foreach (var block in invalidBlocks1)
                    Console.WriteLine($"{block}");
            }
        }

        /// <summary>
        /// Gets a random block from the blockchain, with option to exclude block at a specified index.
        /// </summary>
        /// <param name="blockchain">The blockchain to obtain a random block from.</param>
        /// <param name="excludeBlock">Optional index of block to exclude.</param>
        /// <returns>A random block from the blockchain.</returns>
        private static int GetRandomBlock(BlockchainBase blockchain, int? excludeBlock = null)
        {
            var blocks = blockchain.Chain.Count() - 1;
            var ranBlock = new Random().Next(0, blocks);

            if (excludeBlock != null && ranBlock == excludeBlock)
            {
                // Get a different block no
                if (ranBlock + 1 <= blocks)
                    return ranBlock++;
                else if (ranBlock - 1 >= 0)
                    return ranBlock--;
            }

            return ranBlock;
        }
    }
}
