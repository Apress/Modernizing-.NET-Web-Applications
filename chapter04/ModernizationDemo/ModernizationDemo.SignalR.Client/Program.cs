using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ModernizationDemo.SignalR.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var hubConnection = new HubConnection("https://localhost:44382/"))
            {
                hubConnection.Headers.Add("X-Api-Key", "test-key-1");
                var chat = hubConnection.CreateHubProxy("ChatHub");

                // ask for room id
                var roomId = GetRoomNumber();

                // connect to the hub
                chat.On<string, string>("addChatMessage", (name, message) =>
                {
                    Console.WriteLine($"{name}: {message}");
                }); 
                await hubConnection.Start();

                // join the room
                await chat.Invoke("JoinRoom", roomId);

                while (true)
                {
                    var command = Console.ReadLine()?.Trim() ?? "";
                    if (command.StartsWith("/exit"))
                    {
                        break;
                    }
                    else if (command.StartsWith("/name"))
                    {
                        var parts = command.Split(' ');
                        if (parts.Length != 2)
                        {
                            Console.WriteLine("Invalid syntax.");
                            continue;
                        }
                        await chat.Invoke("SetName", roomId, parts[1]);
                    }
                    else if (!command.StartsWith("/"))
                    {
                        await chat.Invoke("SendMessage", roomId, command);
                    }
                    else
                    {
                        Console.WriteLine("Invalid command.");
                    }
                }
            }
        }

        private static string GetRoomNumber()
        {
            Console.WriteLine("Enter room number (1-3): ");
            var roomIdText = Console.ReadLine();
            if (!int.TryParse(roomIdText, out var roomId) || roomId < 1 || roomId > 3)
            {
                Console.WriteLine("Invalid room number.");
                Environment.Exit(1);
            }
            return $"room{roomId}";
        }
    }
}
