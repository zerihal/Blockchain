namespace BlockchainUtils.Blocks
{
    public interface IPoWBlock
    {
        int Nonce { get; set; }
        void Mine(int difficulty);
    }
}
