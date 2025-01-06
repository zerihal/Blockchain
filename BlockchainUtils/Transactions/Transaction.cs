
namespace BlockchainUtils.Transactions
{
    public class Transaction : IEquatable<Transaction?>
    {
        /// <summary>
        /// Unique ID for this transaction.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// From address (if null then this transaction will be a reward transaction for a miner who
        /// processed a transaction block in the blockchain).
        /// </summary>
        public string? FromAddress { get; set; }

        /// <summary>
        /// To address.
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// Amount of the transaction.
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="fromAddress">From address (can be null for reward).</param>
        /// <param name="toAddress">To address.</param>
        /// <param name="reward">Reward amount (amount of the transaction).</param>
        public Transaction(string? fromAddress, string toAddress, int reward)
        {
            Id = Guid.NewGuid();
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = reward;
        }

        #region Equality overrides
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
        #endregion
    }
}
