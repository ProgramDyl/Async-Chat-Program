using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary;

public class TcpServer
{
    private int _port = 5000;
    private TcpListener tcpListener;

    public void Start()
    {
        IPAddress ipAddress = IPAddress.Any;
        tcpListener = new TcpListener(ipAddress, _port);
        tcpListener.Start();
        Console.WriteLine("Server started on port " + _port);

        while (true)
        {
            TcpClient client = tcpListener.AcceptTcpClient();
            Task.Run(() => HandleClient(client));
        }
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[256];
        int bytesRead;

        while (true)
        {
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + data);

                byte[] responseBuffer = Encoding.ASCII.GetBytes("Echo: " + data);
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
}