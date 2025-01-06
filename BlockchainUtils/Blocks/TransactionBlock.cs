using BlockchainUtils.Transactions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainUtils.Blocks
{
    public class TransactionBlock : PoWBlock, ITransactionBlock
    {
        /// <inheritdoc/>
        public IList<Transaction> Transactions { get; set; }

        public TransactionBlock(DateTime timeStamp, string? previousHash, IEnumerable<Transaction> transactions) : base(timeStamp, previousHash, string.Empty)
        {
            Transactions = new List<Transaction>(transactions);
        }

        /// <inheritdoc/>
        public override string CalculateHash()
        {
            HashAlgorithm sha = BlockchainSettings.CurrentHash;

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}-{Nonce}");
            byte[] outputBytes = sha.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}
