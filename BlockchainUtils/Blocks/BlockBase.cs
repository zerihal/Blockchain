namespace BlockchainUtils.Blocks
{
    public abstract class BlockBase : IBlock
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }

        public abstract string CalculateHash();
    }
}
