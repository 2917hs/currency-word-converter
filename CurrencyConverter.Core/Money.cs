namespace CurrencyConverter.Core;

public readonly record struct Money
{
    public const int MaxFraction = 99;
    public const long MaxWholeNumber = 999_999_999;

    public int Cents { get; }
    public long WholeNumber { get; }

    public Money(int cents, long wholeNumber)
    {
        if (cents is < 0 or > MaxFraction)
        {
            throw new ArgumentOutOfRangeException(nameof(cents));
        }

        if (wholeNumber is < 0 or > MaxWholeNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(wholeNumber));
        }

        Cents = cents;
        WholeNumber = wholeNumber;
    }
}