using BlockchainUtils.Blockchains;
using Newtonsoft.Json;

namespace BlockchainNetworkP2P
{
    public static class Sandbox
    {
        /// <summary>
        /// Sample transaction blockchain for P2P tests.
        /// </summary>
        public static TransactionBlockchain SampleTransactionBlockchain { get; set; } = new TransactionBlockchain(true);

        /// <summary>
        /// Count of server messages that have been processed.
        /// </summary>
        /// <remarks>
        /// Note: This has to be maintained as a static variable due to the async nature of the server sessions.
        /// </remarks>
        public static int ServerMessagesProcessed { get; set; }

        /// <summary>
        /// JSON serializer settings.
        /// </summary>
        public static JsonSerializerSettings JsonSettings => new JsonSerializerSettings() 
        { 
            TypeNameHandling = TypeNameHandling.Auto, 
            Formatting = Formatting.Indented 
        };
    }
}
