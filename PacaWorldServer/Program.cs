using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace PacaWorldServer
{
    class Program
    {

        static short serverPort = 3737;

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Yellow;

            var config = new NetPeerConfiguration("PacaWorld")
            { Port = serverPort };
            var server = new NetServer(config);
            server.Start();
            Console.Clear();
            Console.WriteLine("PacaWorld Server started on " + serverPort.ToString());

            PacaServer();
        }
        static void PacaServer()
        {
            while (true)
            {
                Command();
            }
        }
        static void Command()
        {
            String command;
            command = Console.ReadLine();
            if (command == "clear")
            {
                Console.Clear();
            }

            if (command == "reload")
            {
                PacaServer();
            }

            if (command == "stop")
            {
                Environment.Exit(1);
            }
        }
    }
}
