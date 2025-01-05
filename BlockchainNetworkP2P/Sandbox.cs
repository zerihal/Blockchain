using BlockchainUtils.Blockchains;
using Newtonsoft.Json;

namespace BlockchainNetworkP2P
{
    public static class Sandbox
    {
        public static TransactionBlockchain SampleTransactionBlockchain { get; set; } = new TransactionBlockchain(true);

        public static int P2PMessagesProcessedCount { get; set; }

        public static JsonSerializerSettings JsonSettings => new JsonSerializerSettings() 
        { 
            TypeNameHandling = TypeNameHandling.Auto, 
            Formatting = Formatting.Indented 
        };
    }
}
