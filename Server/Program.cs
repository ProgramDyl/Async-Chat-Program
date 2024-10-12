using System;
using ChatLibrary;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new TcpServer();
            server.Start();
        }
    }
}