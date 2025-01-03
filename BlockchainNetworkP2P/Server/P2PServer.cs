using WebSocketSharp.Server;

namespace BlockchainNetworkP2P.Server
{
    public class P2PServer : WebSocketBehavior
    {
        private const string _address = "ws://localhost";
        private const string _socketService = "/Blockchain";
        private WebSocketServer? wss = null;

        public string ServerAddress { get; private set; } = string.Empty;

        public string SocketServiceAddress { get; private set; } = string.Empty;

        public void Start(int port)
        {
            ServerAddress = $"{_address}:{port}";
            SocketServiceAddress = $"{ServerAddress}{_socketService}";

            wss = new WebSocketServer(ServerAddress);
            wss.AddWebSocketService<BlockchainBehaviour>(_socketService);
            wss.Start();
            Console.WriteLine($"Started server at {ServerAddress}");
        }

        public void Stop() => wss?.Stop();

        // Example of a broadcast message to all clients ...
        public void SendTestMessage()
        {
            if (wss != null)
            {
                foreach (var host in wss.WebSocketServices.Hosts)
                    host.Sessions.Broadcast(Sandbox.Test);
            }
        }
    }
}
