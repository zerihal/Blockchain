using System.Security.Cryptography;
using System.Text;

namespace BlockchainUtils.Blocks
{
    /// <summary>
    /// Proof of Work block - the hash of the previous block must be found (or mined) in order for a new block to be added, 
    /// which will may take a considerable amount of processing time depending on the difficulty set for the blockchain.
    /// </summary>
    public class PoWBlock : BlockBase, IPoWBlock
    {
        /// <inheritdoc/>
        public int Nonce { get; set; } = 0;

        public PoWBlock(DateTime timeStamp, string? previousHash, string data)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }

        /// <inheritdoc/>
        public override string CalculateHash()
        {
            HashAlgorithm sha = BlockchainSettings.CurrentHash;

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}-{Nonce}");
            byte[] outputBytes = sha.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        /// <inheritdoc/>
        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while (Hash == null || Hash.Substring(0, difficulty) != leadingZeros)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}
