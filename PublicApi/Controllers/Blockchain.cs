using Application.Contracts;
using Application.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Blockchain : ControllerBase
    {
        private readonly IBlockchain blockchain;

        public Blockchain(IBlockchain blockchain)
        {
            this.blockchain = blockchain;
        }

        [HttpGet("getBlocks")]
        public ActionResult<IList<Block>> GetBlocks()
            => Ok(blockchain.GetBlocks());

        [HttpPost]
        public ActionResult<string> AddBlock(string data)
        {
            blockchain.AddBlock(data);

            return Ok(data);
        }

        [HttpGet("isValid")]
        public ActionResult<bool> IsValid()
            => Ok(blockchain.IsValid());

        [HttpPut]
        public IActionResult InvalidateBlock(int index, string data)
        {
            blockchain.InvalidateBlock(index, data);

            return Ok();
        }
    }
}
