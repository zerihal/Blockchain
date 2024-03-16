namespace BlockchainUtils.Transactions
{
    public class Transaction
    {
        public string? FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int Amount { get; set; }

        public Transaction(string? fromAddress, string toAddress, int reward)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = reward;
        }
    }
}
