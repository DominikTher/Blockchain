using Application.Entities;

namespace Application.Contracts;

public interface IBlockchain
{
    void AddBlock(IEnumerable<Transaction> transactions);
    IList<Block> GetBlocks();
    bool IsValid();
    void InvalidateBlock(int index, string data);
    void CreateTransaction(Transaction transaction);
    void ProcessPendingTransactions(string minerAddress);
}
