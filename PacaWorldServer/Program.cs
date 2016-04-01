using System;
using System.Collections.Generic;
using System.Xml;
using Lidgren.Network;
using System.Threading;
using System.IO;

namespace PacaWorldServer
{
    class Program
    {
        //server object
        static NetServer Server;
        //configuration object
        static NetPeerConfiguration Config;

        static List<String[]> players = new List<String[]>();

        static void Main(string[] args)
        {
            //sets default color values in case they don't load from the settings file
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            //sets default port in case it doesn't load from the settings file
            short serverPort = 3737;

            //sets the maximum amount of clients in case it doesn't load from the settings file
            byte clientAmount = 16;

            //creates dictionary of color values
            Dictionary<string, ConsoleColor> colorDictionary = new Dictionary<string, ConsoleColor>();

            //adds words for color values
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

            //tries to read xml file
            try
            {
                //reads from settings.emy file
                XmlReader xmlReader = XmlReader.Create("settings.emy");
                //reads through each line
                while (xmlReader.Read())
                {
                    //checks for node named settings 
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "settings"))
                    {
                        //if it finds any attributes
                        if (xmlReader.HasAttributes)
                        {
                            //if the type is listed as a background color
                            if (xmlReader.GetAttribute("type") == "backgroundColor")
                            {
                                //if the color dictionary has the color value
                                if (colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    //gets the color value from the dictionary and sets the background color
                                    Console.BackgroundColor = colorDictionary[xmlReader.GetAttribute("value")];
                                    //outputs the background color
                                    Console.WriteLine("Background color is "+xmlReader.GetAttribute("value"));
                                    //pauses for a second
                                    System.Threading.Thread.Sleep(1000);
                                    //clears the console
                                    Console.Clear();
                                }
                                //if the color dictionary does not have the color value
                                if (!colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    //lets the user know the color value is not in the dictionary
                                    Console.WriteLine("BACKGROUND COLOR INVALID!");
                                    //pauses for a second
                                    System.Threading.Thread.Sleep(1000);
                                    //clears the console
                                    Console.Clear();
                                }
                            }
                            //if the type is listed as a foreground color
                            if (xmlReader.GetAttribute("type") == "foregroundColor")
                            {
                                //if the color dictionary has the color value
                                if (colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    //gets the color value from the dictionary and sets the foreground color
                                    Console.ForegroundColor = colorDictionary[xmlReader.GetAttribute("value")];
                                    //outputs the foreground color
                                    Console.WriteLine("Text color is " + xmlReader.GetAttribute("value"));
                                    //pauses for a second
                                    System.Threading.Thread.Sleep(1000);
                                    //clears the console
                                    Console.Clear();
                                }
                                //if the color dictionary does not have the color value
                                if (!colorDictionary.ContainsKey(xmlReader.GetAttribute("value")))
                                {
                                    //lets the user know the color value is not in the dictionary
                                    Console.WriteLine("TEXT COLOR INVALID!");
                                    //pauses for a second
                                    System.Threading.Thread.Sleep(1000);
                                    //clears the console
                                    Console.Clear();
                                }
                            }
                            //if the type is listed as port
                            if (xmlReader.GetAttribute("type") == "serverPort")
                            {
                                //reads in port number and converts it to short
                                serverPort = Convert.ToInt16(xmlReader.GetAttribute("value"));
                                //outputs the port
                                Console.WriteLine("Server port is " + xmlReader.GetAttribute("value"));
                                //pauses for a second
                                System.Threading.Thread.Sleep(1000);
                                //clears the console
                                Console.Clear();
                            }
                            //if the type is listed as the maximum amount of clients
                            if (xmlReader.GetAttribute("type") == "clientAmount")
                            {
                                //reads in maximum amount of clients and converts it to byte
                                clientAmount = Convert.ToByte(xmlReader.GetAttribute("value"));
                                //outputs the maximum amount of clients
                                Console.WriteLine("Maximum amount of clients is " + xmlReader.GetAttribute("value"));
                                //pauses for a second
                                System.Threading.Thread.Sleep(1000);
                                //clears the console
                                Console.Clear();
                            }
                        }   
                    }
                }
                //tells the user the settings file was loaded
                Console.WriteLine("SETTINGS FILE LOADED");
            }
            //catches any errors
            catch (Exception e)
            {
                //let the user know what went wrong
                Console.WriteLine("SETTINGS FILE COULD NOT BE LOADED " + e.Message);
            }
            //create instance of configs
            Config = new NetPeerConfiguration("PacaWorld");
            //sets the server port
            Config.Port = serverPort;
            //sets the maximum client amount
            Config.MaximumConnections = clientAmount;
            //create new server based on the configs just defined
            Server = new NetServer(Config);
            //start the server
            Server.Start();
            //output server start
            Console.WriteLine("Server Started");

            //goes into the PacaServer() function
            PacaServer();
        }

        static void PacaServer()
        {
            //starts new thread for command checker
            new Thread(() =>
            {
                //loops command checker
                while(true)
                {
                    //runs the command checker
                    Command();
                }
            }).Start();
            //infinite loop
            while (true)
            {
                if (Server.ConnectionsCount > 0)
                {
                    try
                    {
                        //read for incoming messages
                        NetIncomingMessage newMessage = Server.ReadMessage();

                        if (newMessage != null)
                        {
                            //read messages
                            string receivedMessage = newMessage.ReadString();
                            //read out the objects
                            XmlReader xmlReader = XmlReader.Create(new StringReader(receivedMessage));
                            while (xmlReader.Read())
                            {
                                if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "object"))
                                {
                                    if (xmlReader.HasAttributes && xmlReader.GetAttribute("type") == "player")
                                    {
                                        //create new player
                                        CreatePlayer(xmlReader.GetAttribute("name"), Int32.Parse(xmlReader.GetAttribute("x")), Int32.Parse(xmlReader.GetAttribute("y")), xmlReader.GetAttribute("posture"));
                                        //todo announce that a player has joined if a new one is created
                                    }
                                }
                            }
                            Console.WriteLine("Received message!");
							NetOutgoingMessage sendMsg = Server.CreateMessage();
							sendMsg.Write(GetPlayerXML());
							Server.SendMessage(sendMsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.ToString().IndexOf("recipients must contain at least one item") != -1)
                        {
                            continue;
                        }
                        else if (e.ToString().IndexOf("System.Xml.XmlException") != -1)
                        {
                            Console.WriteLine("Invalid message received!");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("ERROR:" + e);
                            break;
                        }
                    }
                }
            }
        }

        //function to create players
        static void CreatePlayer(string name, int x, int y, string posture)
        {
            for (int i=0; i<players.Count; i++)
            {
                //if the player already exists then it just breaks
                if(players[i][0]==name)
                {
                    break;
                }
                //if the player does not exist then it adds them
                if (i >= players.Count)
                {
                    string[] player;
                    player = new string[4];
                    player[0] = name;
                    player[1] = x.ToString();
                    player[2] = y.ToString();
                    player[3] = posture;
                    players.Add(player);
                }
            }
        }

        //changes the players x and y coords 
        static void ChangePlayerCoords(string name, int x, int y)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i][0] == name)
                {
                    players[i][1] = x.ToString();
                    players[i][2] = y.ToString();
                }
            }
        }

        static string GetPlayerXML()
        {
            //creates value to hold xml values
            StringWriter playerCoords = new StringWriter();
            XmlWriter playerCoordinates = XmlWriter.Create(playerCoords);
            playerCoordinates.WriteStartDocument();
            playerCoordinates.WriteStartElement("object");
            for (int i = 0; i < players.Count; i++)
            {
                playerCoordinates.WriteStartElement("object");
                playerCoordinates.WriteAttributeString("type", "player");
                playerCoordinates.WriteAttributeString("name", players[i][0]);
                playerCoordinates.WriteAttributeString("x", players[i][1]);
                playerCoordinates.WriteAttributeString("y", players[i][2]);
                playerCoordinates.WriteAttributeString("posture", players[i][3]);
                playerCoordinates.WriteEndElement();
            }
            playerCoordinates.WriteEndDocument();
            playerCoordinates.Close();
            string returnedPlayerCoords = playerCoords.ToString();
            playerCoords.GetStringBuilder().Length = 0;
            return returnedPlayerCoords;
        }

        static void Command()
        {
            //creates string to hold input
            String command;
            //reads in any text
            command = Console.ReadLine();
            //if the command is equal to clear
            if (command == "clear")
            {
                //clear the console
                Console.Clear();
            }
            //if the command is equal to hi
            if (command == "hi")
            {
                //outputs Hello!
                Console.WriteLine("Hello!");
            }
            //if the command is equal to stop or exit
            if (command == "stop" || command == "exit")
            {
                //stops the application
                Environment.Exit(1);
            }
            //logs current players to file
            if (command=="generatexml")
            {
                string outputName;

                //gets output file name
                outputName = Console.ReadLine()+".emy";

                //generates xml
                try
                {
                    XmlWriter xmlWriter = XmlWriter.Create(outputName);
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("object");

                    for (int i = 0; i < players.Count; i++)
                    {
                        xmlWriter.WriteStartElement("object");
                        xmlWriter.WriteAttributeString("type", "player");
                        xmlWriter.WriteAttributeString("name", players[i][0]);
                        xmlWriter.WriteAttributeString("x", players[i][1]);
                        xmlWriter.WriteAttributeString("y", players[i][2]);
                        xmlWriter.WriteAttributeString("posture", players[i][3]);
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndDocument();
                    xmlWriter.Close();
                    XmlReader xmlReader = XmlReader.Create(outputName);
                    while (xmlReader.Read())
                    {
                        if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "object"))
                        {
                            if (xmlReader.HasAttributes)
                                Console.WriteLine(xmlReader.GetAttribute("type") + ":" + xmlReader.GetAttribute("name") + ":" + xmlReader.GetAttribute("x") + ":" + xmlReader.GetAttribute("y") + ":" + xmlReader.GetAttribute("posture"));
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("ERROR:" + e);
                }
            }
        }
    }
}
