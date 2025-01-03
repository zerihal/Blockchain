namespace BlockchainUtils.Blocks
{
    public abstract class BlockBase : IBlock
    {
        /// <inheritdoc/>
        public int Index { get; set; }

        /// <inheritdoc/>
        public DateTime TimeStamp { get; set; }

        /// <inheritdoc/>
        public string? PreviousHash { get; set; }

        /// <inheritdoc/>
        public string Hash { get; set; }

        /// <inheritdoc/>
        public string Data { get; set; }

        /// <inheritdoc/>
        public abstract string CalculateHash();
    }
}
