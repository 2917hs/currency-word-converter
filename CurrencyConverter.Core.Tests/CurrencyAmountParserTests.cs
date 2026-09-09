using CurrencyConverter.Core.BusinessRules;
using CurrencyConverter.Core.Exceptions;

namespace CurrencyConverter.Core.Tests;

public class CurrencyAmountParserTests
{
    [Theory]
    [InlineData("0", 0, 0L)]
    [InlineData("1", 0, 1L)]
    [InlineData("25,1", 10, 25L)]
    [InlineData("0,01", 1, 0L)]
    [InlineData("45 100", 0, 45100L)]
    [InlineData("999 999 999,99", 99, 999_999_999L)]
    public void Parse_ReturnsExpectedMoney_ForValidInput(string input, int expectedCents, long expectedWholeNumber)
    {
        var money = CurrencyAmountParser.Parse(input);

        Assert.Equal(expectedCents, money.Cents);
        Assert.Equal(expectedWholeNumber, money.WholeNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("25,1,5")]
    [InlineData("5,999")]
    [InlineData("1000000000")]
    public void Parse_Throws_ForInvalidInput(string? input)
    {
        Assert.Throws<ValidationException>(() => CurrencyAmountParser.Parse(input));
    }
}
