using Application.Entities;

namespace Application.Contracts
{
    public interface IBlockchain
    {
        void AddBlock(string data);
        IList<Block> GetBlocks();
        bool IsValid();
        void InvalidateBlock(int index, string data);
    }
}
