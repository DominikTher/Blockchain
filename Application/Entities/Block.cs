using System.Security.Cryptography;
using System.Text;

namespace Application.Entities
{
    public sealed record Block
    {
        public int Index { get; init; }
        public DateTime TimeStamp { get; init; }
        public string Hash { get; set; } = string.Empty;
        public string PreviousHash { get; init; } = string.Empty;
        public string Data { get; set; } = string.Empty;

        private int nonce;

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}-{nonce}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }

        public void Mine(int proofOfWork)
        {
            var leadingZeros = new string('0', proofOfWork);
            while (string.IsNullOrWhiteSpace(Hash) || Hash.Substring(0, proofOfWork) != leadingZeros)
            {
                nonce++;
                Hash = CalculateHash();
            }
        }
    }
}
