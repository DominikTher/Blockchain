using Application.Contracts;
using Application.Entities;

namespace Application
{
    public sealed class Blockchain : IBlockchain
    {
        private readonly IList<Block> blocks = new List<Block> { new() };

        public void AddBlock(string data)
        {
            var latestBlock = blocks.Last();
            var block = new Block
            {
                Index = latestBlock.Index + 1,
                PreviousHash = latestBlock.Hash,
                Data = data,
                TimeStamp = DateTime.UtcNow,
            };

            blocks.Add(block with { Hash = block.CalculateHash() });
        }

        public IList<Block> GetBlocks() 
            => blocks;

        public void InvalidateBlock(int index, string data) 
            => blocks.ElementAt(index).Data = data;

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

                if(currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
