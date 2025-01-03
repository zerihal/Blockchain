using System.Security.Cryptography;
using System.Text;

namespace BlockchainUtils.Blocks
{
    /// <summary>
    /// Basic hash linked block, immutably linked to the previous block in the blockchain.
    /// </summary>
    public class BasicBlock : BlockBase
    {
        public BasicBlock(DateTime timeStamp, string? previousHash, string data)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }

        /// <summary>
        /// Calculates the hash based on the previous block in the chain (if present), using its timestamp,
        /// hash, and data.
        /// </summary>
        /// <returns>Calculated hash as string.</returns>
        public override string CalculateHash()
        {
            HashAlgorithm sha = BlockchainSettings.CurrentHash;

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");
            byte[] outputBytes = sha.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}
