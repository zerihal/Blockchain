using System.Security.Cryptography;
using System.Text;

namespace BlockchainUtils.Blocks
{
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

        public override string CalculateHash()
        {
            HashAlgorithm sha = BlockchainSettings.CurrentHash;

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");
            byte[] outputBytes = sha.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}
