using System.Security.Cryptography;

namespace BlockchainUtils
{
    /// <summary>
    /// Global settings for all Blockchain implementations.
    /// </summary>
    public static class BlockchainSettings
    {
        public static int MineDifficulty { get; set; } = 3;

        public static HashAlorithmImp BlockchainHashAlgorithm { get; set; }

        public static HashAlgorithm CurrentHash
        {
            get
            {
                switch (BlockchainHashAlgorithm)
                {
                    case HashAlorithmImp.SHA384:
                        return SHA384.Create();

                    case HashAlorithmImp.SHA512:
                        return SHA512.Create();

                    case HashAlorithmImp.SHA1:
                        return SHA1.Create();

                    case HashAlorithmImp.SHA256:
                    default:
                        return SHA256.Create();
                }
            }
        }
    }
}
