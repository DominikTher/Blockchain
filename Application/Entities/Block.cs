using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Application.Entities;

public sealed record Block
{
    public int Index { get; init; }
    public DateTime TimeStamp { get; init; }
    public string Hash { get; set; } = string.Empty;
    public string PreviousHash { get; init; } = string.Empty;
    public IEnumerable<Transaction> Transactions { get; init; } = Enumerable.Empty<Transaction>();
    public int Nonce { get; set; }

    public string CalculateHash()
    {
        SHA256 sha256 = SHA256.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonSerializer.Serialize(Transactions)}-{Nonce}");
        byte[] outputBytes = sha256.ComputeHash(inputBytes);

        return Convert.ToBase64String(outputBytes);
    }

    public void Mine(int proofOfWork)
    {
        var leadingZeros = new string('0', proofOfWork);
        do
        {
            Nonce++;
            Hash = CalculateHash();
        } while (Hash[..proofOfWork] != leadingZeros);
    }
}
