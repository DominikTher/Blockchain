using Application;
using Application.Entities;
using System.Text.Json;
using WebSocketSharp;

public class Program
{
    public static Blockchain Blockchain = new Blockchain();

    public static void Main(string[] args)
    {
        var c = new P2PClient();
        c.Connect("ws://127.0.0.1:1234/Example");
        Console.WriteLine("Connected");
        Console.ReadLine();

        Blockchain.CreateTransaction(new Transaction
        {
            Amount = 100,
            FromAddress = "A",
            ToAddress = "B"
        });
        Blockchain.CreateTransaction(new Transaction
        {
            Amount = 50,
            FromAddress = "C",
            ToAddress = "D"
        });
        Blockchain.ProcessPendingTransactions("Dominik");

        c.Broadcast(JsonSerializer.Serialize(Blockchain));
        Console.WriteLine("Sent");
        Console.ReadLine();
    }
}

public class P2PClient
{
    IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();

    public void Connect(string url)
    {
        if (!wsDict.ContainsKey(url))
        {
            WebSocket ws = new WebSocket(url);
            ws.OnMessage += (sender, e) =>
            {
                Blockchain newChain = JsonSerializer.Deserialize<Blockchain>(e.Data) ?? new Blockchain();
                if (newChain.IsValid() && newChain.blocks.Count > Program.Blockchain.blocks.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.pendingTransactions);
                    newTransactions.AddRange(Program.Blockchain.pendingTransactions);

                    newChain.pendingTransactions = newTransactions;
                    Program.Blockchain = newChain;

                    Console.WriteLine("Received");
                    Console.WriteLine(JsonSerializer.Serialize(Program.Blockchain));
                }

            };
            ws.Connect();
            ws.Send(JsonSerializer.Serialize(Program.Blockchain));
            wsDict.Add(url, ws);
        }
    }

    public void Broadcast(string data)
    {
        foreach (var item in wsDict)
        {
            item.Value.Send(data);
        }
    }
}