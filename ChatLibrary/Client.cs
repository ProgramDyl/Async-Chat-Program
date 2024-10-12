using System.Net.Sockets;
using System.Text;

namespace ChatLibrary;

public class TcpClientApp
{
    private readonly int _port = 5000;
    private readonly string _serverIp = "127.0.0.1";
    private TcpClient client;
    private NetworkStream stream;

    public event EventHandler<string> MessageReceived;

    public void Start()
    {
        try
        {
            client = new TcpClient(_serverIp, _port);
            stream = client.GetStream();
            Task.Run(() => ReceiveMessages());
        }
        catch (Exception e)
        {
            OnMessageReceived($"Error: {e.Message}");
        }
    }

    private void ReceiveMessages()
    {
        var buffer = new byte[256];
        int bytesRead;

        while (true)
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                var responseData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                OnMessageReceived(responseData);
            }
            catch (Exception e)
            {
                OnMessageReceived($"Error: {e.Message}");
                break;
            }

        stream?.Close();
        client?.Close();
    }

    public void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        var data = Encoding.ASCII.GetBytes(message);
        stream?.Write(data, 0, data.Length);
    }

    protected virtual void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(this, message);
    }

    public void Close()
    {
        stream?.Close();
        client?.Close();
    }
}