using System;
using ChatLibrary;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClientApp client = new TcpClientApp();
            client.Start();
        }
    }
}