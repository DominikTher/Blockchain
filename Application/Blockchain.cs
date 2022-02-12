using Application.Contracts;
using Application.Entities;

namespace Application;

public sealed class Blockchain : IBlockchain
{
    private readonly IList<Block> blocks = new List<Block> { new() };
    private readonly int proofOfWork = 2;
    private readonly int reward = 1;

    private IList<Transaction> pendingTransactions = new List<Transaction>();

    public void AddBlock(IEnumerable<Transaction> transactions)
    {
        var latestBlock = blocks.Last();
        var block = new Block
        {
            Index = latestBlock.Index + 1,
            PreviousHash = latestBlock.Hash,
            Transactions = transactions,
            TimeStamp = DateTime.UtcNow,
        };

        block.Mine(proofOfWork);

        blocks.Add(block with { Hash = block.CalculateHash() });
    }

    public void CreateTransaction(Transaction transaction) 
        => pendingTransactions.Add(transaction);

    public IList<Block> GetBlocks()
        => blocks;

    public void InvalidateBlock(int index, string data)
    {
        // blocks.ElementAt(index).Data = data;
        throw new NotImplementedException();
    }

    public bool IsValid()
    {
        for (int index = 1; index < blocks.Count; index++)
        {
            var currentBlock = blocks[index];
            var previousBlock = blocks[index - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }

        return true;
    }

    public void ProcessPendingTransactions(string minerAddress)
    {
        AddBlock(pendingTransactions);
        pendingTransactions = new List<Transaction>();
        CreateTransaction(new Transaction
        {
            FromAddress = string.Empty,
            ToAddress = minerAddress,
            Amount = reward
        });
    }
}
