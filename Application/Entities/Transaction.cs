namespace Application.Entities;

public sealed record Transaction
{
    public string FromAddress { get; init; } = string.Empty;
    public string ToAddress { get; init; } = string.Empty;
    public int Amount { get; init; }
}
