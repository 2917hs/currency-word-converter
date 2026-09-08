namespace CurrencyToWordConverter.Core;

public readonly record struct Money(int Cent, long Amount)
{
    public const int MaxCents = 99;
    public const int MaxAmount = 999_999_999;
}