using Microsoft.AspNetCore.SignalR.Client;

await using var chat = new HubConnectionBuilder()
    .WithUrl("https://localhost:7201/hubs/ChatHub", options =>
    {
        options.Headers.Add("X-Api-Key", "test-key-1");
    })
    .WithAutomaticReconnect(new ForeverRetryPolicy())
    .Build();

// ask for room id
var roomId = GetRoomNumber();

// connect to the hub
chat.On<string, string>("addChatMessage", (name, message) =>
{
    Console.WriteLine($"{name}: {message}");
});
await chat.StartAsync();

// join the room
await chat.InvokeAsync("JoinRoom", roomId);

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
        await chat.InvokeAsync("SetName", roomId, parts[1]);
    }
    else if (!command.StartsWith("/"))
    {
        await chat.InvokeAsync("SendMessage", roomId, command);
    }
    else
    {
        Console.WriteLine("Invalid command.");
    }
}

string GetRoomNumber()
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

public class ForeverRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        return TimeSpan.FromSeconds(5);
    }
}