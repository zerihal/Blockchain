namespace BlockchainUtils.Blocks
{
    public interface IBlock
    {
        /// <summary>
        /// Block data.
        /// </summary>
        string Data { get; set; }

        /// <summary>
        /// Block hash.
        /// </summary>
        string Hash { get; set; }

        /// <summary>
        /// Block index in chain.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Hash of the previous block in the chain.
        /// </summary>
        string? PreviousHash { get; set; }

        /// <summary>
        /// Timestamp of block creation.
        /// </summary>
        DateTime TimeStamp { get; set; }

        /// <summary>
        /// Calculates the hash.
        /// </summary>
        /// <returns></returns>
        string CalculateHash();
    }
}