using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using Lidgren.Network;
using Newtonsoft.Json;

namespace PacaWorldServer
{
    class Program
    {

        static short serverPort = 3737;

        static void Main(string[] args)
        {

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.Clear();

            Dictionary<string, ConsoleColor> colorDictionary = new Dictionary<string, ConsoleColor>();

            colorDictionary.Add("red", ConsoleColor.Red);
            colorDictionary.Add("darkred", ConsoleColor.DarkRed);
            colorDictionary.Add("green", ConsoleColor.Green);
            colorDictionary.Add("darkgreen", ConsoleColor.DarkGreen);
            colorDictionary.Add("blue", ConsoleColor.Blue);
            colorDictionary.Add("darkblue", ConsoleColor.DarkBlue);
            colorDictionary.Add("yellow", ConsoleColor.Yellow);
            colorDictionary.Add("darkyellow", ConsoleColor.DarkYellow);
            colorDictionary.Add("magenta", ConsoleColor.Magenta);
            colorDictionary.Add("darkmagenta", ConsoleColor.DarkMagenta);
            colorDictionary.Add("gray", ConsoleColor.Gray);
            colorDictionary.Add("darkgray", ConsoleColor.DarkGray);
            colorDictionary.Add("cyan", ConsoleColor.Cyan);
            colorDictionary.Add("darkcyan", ConsoleColor.DarkCyan);
            colorDictionary.Add("white", ConsoleColor.White);
            colorDictionary.Add("black", ConsoleColor.Black);

            try
            {
                XmlReader xmlReader = XmlReader.Create("settings.emy");
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "settings"))
                    {
                        if (xmlReader.HasAttributes)
                        {
                            if(xmlReader.GetAttribute("type")=="port")
                            {
                                serverPort = Convert.ToInt16(xmlReader.GetAttribute("value"));
                                Console.WriteLine("Port is " + xmlReader.GetAttribute("value"));
                                System.Threading.Thread.Sleep(1000);
                                Console.Clear();
                            }
                            if (xmlReader.GetAttribute("type") == "backgroundColor")
                            {
                                if (colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    Console.BackgroundColor = colorDictionary[xmlReader.GetAttribute("value")];
                                    Console.WriteLine("Background color is "+xmlReader.GetAttribute("value"));
                                    System.Threading.Thread.Sleep(1000);
                                    Console.Clear();
                                }
                                if (!colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    Console.WriteLine("BACKGROUND COLOR INVALID!");
                                    System.Threading.Thread.Sleep(1000);
                                    Console.Clear();
                                }
                            }
                            if (xmlReader.GetAttribute("type") == "foregroundColor")
                            {
                                if (colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    Console.ForegroundColor = colorDictionary[xmlReader.GetAttribute("value")];
                                    Console.WriteLine("Text color is " + xmlReader.GetAttribute("value"));
                                    System.Threading.Thread.Sleep(1000);
                                    Console.Clear();
                                }
                                if (!colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    Console.WriteLine("TEXT COLOR INVALID!");
                                    System.Threading.Thread.Sleep(1000);
                                    Console.Clear();
                                }
                            }
                        }   
                    }
                }
                Console.WriteLine("SETTINGS FILE LOADED");
            }
            catch (Exception e)
            {

                // Let the user know what went wrong.
                Console.WriteLine("SETTINGS FILE COULD NOT BE LOADED " + e.Message);
            }

            var config = new NetPeerConfiguration("PacaWorld")
            { Port = serverPort };
            var server = new NetServer(config);
            server.Start();
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

            if (command == "hi")
            {
                Console.WriteLine("Hello!");
            }

            if (command == "stop")
            {
                Environment.Exit(1);
            }
        }
    }
}
