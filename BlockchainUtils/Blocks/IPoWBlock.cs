namespace BlockchainUtils.Blocks
{
    public interface IPoWBlock
    {
        /// <summary>
        /// Nonce to iteratively increment to generate a different hash until the generated hash matches the previous
        /// block in the chain, according to difficulty.
        /// </summary>
        int Nonce { get; set; }

        /// <summary>
        /// Calculates the hash based for the current block on the previously mined block in the chain (if present), 
        /// using its timestamp, hash, data, and nonce.
        /// </summary>
        /// <returns>Calculated hash as string.</returns>
        void Mine(int difficulty);
    }
}
