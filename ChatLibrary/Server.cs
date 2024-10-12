using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary
{
    public class TcpServer
    {
        private readonly int _port = 5000;
        private TcpListener tcpListener;
        private TcpClient connectedClient;

        public void Start()
        {
            var ipAddress = IPAddress.Any;
            tcpListener = new TcpListener(ipAddress, _port);
            tcpListener.Start();
            Console.WriteLine("Server started on port " + _port);

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                connectedClient = client; // Store the connected client
                Console.WriteLine($"Connected to {((IPEndPoint)client.Client.RemoteEndPoint).Address} from SERVER {((IPEndPoint)tcpListener.LocalEndpoint).Address}");

                Task.Run(() => HandleClient(client));
                Task.Run(() => HandleServerInput()); // Task to handle server input
            }
        }

        private void HandleClient(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[256];
            int bytesRead;

            while (true)
            {
                try
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    var data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + data);

                    var responseBuffer = Encoding.ASCII.GetBytes("You: " + data);
                    stream.Write(responseBuffer, 0, responseBuffer.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    break;
                }
            }

            client.Close();
        }

        private void HandleServerInput()
        {
            while (true)
            {
                string message = Console.ReadLine();
                SendMessageToClient(message);
            }
        }

        private void SendMessageToClient(string message)
        {
            if (connectedClient != null)
            {
                var stream = connectedClient.GetStream();
                var buffer = Encoding.ASCII.GetBytes($"SERVER: {message}");
                stream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
