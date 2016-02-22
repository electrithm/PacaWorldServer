using System;
using System.Collections.Generic;
using System.Xml;
using Lidgren.Network;

namespace PacaWorldServer
{
    class Program
    {
        //sets default port in case it doesn't load from the setting file
        static short serverPort = 3737;

        static void Main(string[] args)
        {
            //sets default color values in case they don't load from the settings file
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

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
                while (xmlReader.Read())
                {
                    //checks for node named settings 
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "settings"))
                    {
                        //if it finds any attributes
                        if (xmlReader.HasAttributes)
                        {
                            //if the type is listed as port
                            if(xmlReader.GetAttribute("type")=="port")
                            {
                                //reads in port number and converts it to short
                                serverPort = Convert.ToInt16(xmlReader.GetAttribute("value"));
                                //outputs the port
                                Console.WriteLine("Port is " + xmlReader.GetAttribute("value"));
                                //pauses for a second
                                System.Threading.Thread.Sleep(1000);
                                //clears the console
                                Console.Clear();
                            }
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

            //sets the app name for lidgren and the port number
            var config = new NetPeerConfiguration("PacaWorld"){ Port = serverPort };
            //sets up the server using the config
            var server = new NetServer(config);
            //starts the server
            server.Start();
            //outputs the port the server started on
            Console.WriteLine("PacaWorld Server started on " + serverPort.ToString());

            //goes into the PacaServer() function
            PacaServer();
        }

        static void PacaServer()
        {
            //infinite loop
            while (true)
            {
                //runs the command checker
                Command();
            }
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
            //if the command is equal to stop
            if (command == "stop")
            {
                //stops the application
                Environment.Exit(1);
            }
        }
    }
}
