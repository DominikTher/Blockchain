using System.Security.Cryptography;
using System.Text;

namespace Application.Entities
{
    public sealed record Block
    {
        public int Index { get; init; }
        public DateTime TimeStamp { get; init; }
        public string Hash { get; init; } = string.Empty;
        public string PreviousHash { get; init; } = string.Empty;
        public string Data { get; set; } = string.Empty;

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }
}
