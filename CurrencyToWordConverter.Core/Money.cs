namespace CurrencyToWordConverter.Core;

public readonly record struct Money(int Decimal, long WholeNumber)
{
    public const int MaxDecimal = 99;
    public const long MaxWholeNumber = 999_999_999;
}