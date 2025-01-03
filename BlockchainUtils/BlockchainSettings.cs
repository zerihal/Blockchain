using System.Security.Cryptography;

namespace BlockchainUtils
{
    /// <summary>
    /// Global settings for all Blockchain implementations.
    /// </summary>
    public static class BlockchainSettings
    {
        /// <summary>
        /// Mining difficulty for PoW and transaction blocks.
        /// </summary>
        public static int MineDifficulty { get; set; } = 3;

        /// <summary>
        /// Blockchain hash algorithm.
        /// </summary>
        public static HashAlorithmImp BlockchainHashAlgorithm { get; set; }

        /// <summary>
        /// Currently set hash algorithm.
        /// </summary>
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
