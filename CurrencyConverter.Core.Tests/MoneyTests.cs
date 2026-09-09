namespace CurrencyConverter.Core.Tests;

public class MoneyTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(100)]
    public void Constructor_Throws_WhenCentsOutOfRange(int invalidCents)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money(invalidCents, 0));
    }

    [Theory]
    [InlineData(-1L)]
    [InlineData(1_000_000_000L)]
    public void Constructor_Throws_WhenWholeNumberOutOfRange(long invalidWholeNumber)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money(0, invalidWholeNumber));
    }
}
