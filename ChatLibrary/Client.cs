using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary;

public class TcpClientApp
{
    private string serverIp = "127.0.0.1";
    private int port = 5000;
    private TcpClient client;
    private NetworkStream stream;

    public void Start()
    {
        try
        {
            client = new TcpClient(serverIp, port);
            stream = client.GetStream();

            Task.Run(() => ReceiveMessages());

            while (true)
            {
                Console.Write("Enter a message to send: ");
                string message = Console.ReadLine();

                if (message == "exit")
                {
                    client.Close();
                    break;
                }

                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    private void ReceiveMessages()
    {
        byte[] buffer = new byte[256];
        int bytesRead;

        while (true)
        {
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string responseData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + responseData);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                break;
            }
        }

        client.Close();
    }
}