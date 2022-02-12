using Application;
using Application.Entities;
using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;
public class Program
{
    public static Blockchain Blockchain = new Blockchain();

    public static void Main(string[] args)
    {
        var wssv = new WebSocketServer($"ws://127.0.0.1:1234");
        wssv.AddWebSocketService<Laputa>("/Example");
        wssv.Start();
        Console.WriteLine($"Started server at ws://127.0.0.1:1234");
        Console.ReadKey();
        wssv.Stop();
    }
}


public class Laputa : WebSocketBehavior
{
    bool chainSynched = false;

    protected override void OnMessage(MessageEventArgs e)
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

        if (!chainSynched)
        {
            Console.WriteLine("Sync");
            Console.WriteLine(JsonSerializer.Serialize(Program.Blockchain));
            Send(JsonSerializer.Serialize(Program.Blockchain));
            chainSynched = true;
        }
    }
}