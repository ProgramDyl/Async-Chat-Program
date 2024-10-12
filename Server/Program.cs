using ChatLibrary;

namespace Server;

internal class Program
{
    private static void Main(string[] args)
    {
        var server = new TcpServer();
        server.Start();
    }
}