using BlockchainUtils.Blockchains;
using BlockchainUtils.Transactions;
using Newtonsoft.Json;

namespace BlockchainNetworkP2P
{
    public static class MessageHelper
    {
        public const string ClientHello = "Hello Client";
        public const string ServerHello = "Hello Server";
        public const string Test = "Test Message";
        public const string NewPendingTransactions = "New Pending Transactions";

        /// <summary>
        /// Serializes the message data as a string or JSON string prepended with the object type.
        /// </summary>
        /// <param name="data">Message data.</param>
        /// <returns>Serialized message string.</returns>
        /// <exception cref="InvalidDataException">Thrown if data object type cannot be determined.</exception>
        public static string SerializeMessage(object data)
        {
            var typeName = data.GetType().AssemblyQualifiedName;

            if (typeName != null)
            {
                if (data is string strData)
                {
                    return $"{typeName}###{strData}";
                }
                else
                {
                    var json = JsonConvert.SerializeObject(data, Sandbox.JsonSettings);
                    return $"{typeName}###{json}";
                }
            }

            throw new InvalidDataException("Invalid data type");
        }

        /// <summary>
        /// Deserializes message back to its original object type.
        /// </summary>
        /// <param name="message">Serialized message.</param>
        /// <param name="msgJsonData">JSON formatted message part.</param>
        /// <returns>Deserialized object from message string.</returns>
        /// <exception cref="InvalidDataException">Thrown if data object type cannot be determined.</exception>
        public static object? DeserializeMessage(string message, out string msgJsonData)
        {
            var splitMsg = message.Split("###");

            if (splitMsg.Length == 2)
            {
                var type = Type.GetType(splitMsg[0]);
                msgJsonData = splitMsg[1];

                if (type != null)
                {
                    if (type == typeof(string))
                        return msgJsonData;
                    else if (type == typeof(TransactionBlockchain))
                        return JsonConvert.DeserializeObject<TransactionBlockchain>(msgJsonData, Sandbox.JsonSettings);
                    else if (type == typeof(Transaction))
                        return JsonConvert.DeserializeObject<Transaction>(msgJsonData, Sandbox.JsonSettings);
                    else
                        return JsonConvert.DeserializeObject(msgJsonData, Sandbox.JsonSettings);
                }
            }

            throw new InvalidDataException("Invalid data type");
        }

        /// <summary>
        /// Tries to deserialize JSON string to the specified type.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="json">JSON string.</param>
        /// <returns>Deserialized object.</returns>
        public static T? TryDeserializeJsonObject<T>(string json)
        {
            try
            {
                if (JsonConvert.DeserializeObject<T>(json, Sandbox.JsonSettings) is T deserializedObj)
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
