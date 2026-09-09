namespace AgileFood.Business.Models.Users;

public static class TransactionPin
{
    public const int Length = 4;

    public static bool IsValid(string? pin)
    {
        return !string.IsNullOrWhiteSpace(pin) &&
                pin.Length == Length &&
                pin.All(char.IsDigit);
    }   
}
