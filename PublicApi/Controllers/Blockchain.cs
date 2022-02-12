using Application.Contracts;
using Application.Entities;
using Microsoft.AspNetCore.Mvc;

namespace PublicApi.Controllers;

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

    //[HttpPost]
    //public ActionResult<string> AddBlock(string data)
    //{
    //    blockchain.AddBlock(data);

    //    return Ok(data);
    //}

    [HttpPost("createTransaction")]
    public IActionResult CreateTransaction(string fromAddress, string toAddress, int amount)
    {
        blockchain.CreateTransaction(new Transaction
        {
            ToAddress = toAddress,
            Amount = amount,
            FromAddress = fromAddress
        });

        return Ok();
    }

    [HttpPost("processPendingTransactions")]
    public IActionResult ProcessPendingTransactions(string minerAddress)
    {
        blockchain.ProcessPendingTransactions(minerAddress);

        return Ok();
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
