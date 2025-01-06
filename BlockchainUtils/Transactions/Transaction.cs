
namespace BlockchainUtils.Transactions
{
    public class Transaction : IEquatable<Transaction?>
    {
        public Guid Id { get; }
        public string? FromAddress { get; set; }
        public string ToAddress { get; set; }
        public int Amount { get; set; }

        public Transaction(string? fromAddress, string toAddress, int reward)
        {
            Id = Guid.NewGuid();
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = reward;
        }

        public override bool Equals(object? obj) => Equals(obj as Transaction);

        public bool Equals(Transaction? other) => other is not null && Id.Equals(other.Id);

        public override int GetHashCode() => HashCode.Combine(Id);

        public static bool operator ==(Transaction? left, Transaction? right)
        {
            return EqualityComparer<Transaction>.Default.Equals(left, right);
        }

        public static bool operator !=(Transaction? left, Transaction? right)
        {
            return !(left == right);
        }
    }
}
