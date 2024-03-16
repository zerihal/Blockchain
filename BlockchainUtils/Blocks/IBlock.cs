namespace BlockchainUtils.Blocks
{
    public interface IBlock
    {
        string Data { get; set; }
        string Hash { get; set; }
        int Index { get; set; }
        string? PreviousHash { get; set; }
        DateTime TimeStamp { get; set; }

        string CalculateHash();
    }
}