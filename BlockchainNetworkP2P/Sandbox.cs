using BlockchainUtils.Blockchains;
using Newtonsoft.Json;

namespace BlockchainNetworkP2P
{
    public static class Sandbox
    {
        public const string ClientHello = "Hello Client";
        public const string ServerHello = "Hello Server";
        public const string Test = "Test Message";

        public static TransactionBlockchain SampleTransactionBlockchain { get; set; } = new TransactionBlockchain(true);

        public static JsonSerializerSettings JsonSettings => new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };

        public static T? TryDeserializeJsonObject<T>(string json)
        {
            try
            {
                if (JsonConvert.DeserializeObject<T>(json, JsonSettings) is T deserializedObj)
                    return deserializedObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing object: {ex.Message}");
            }

            return default;
        }
    }
}
