using CurrencyConverter.Core.BusinessRules.Converters;
using CurrencyConverter.Core.BusinessRules.Interfaces;
using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.Tests;

public class CurrencyToWordsConverterTests
{
    private readonly ICurrencyToWordsConverter _converter = new CurrencyToWordsConverter(
        new IAmountToWordsConverter[] { new EnglishAmountToWordsConverter(), new GermanAmountToWordsConverter() });

    [Theory]
    [InlineData("0", "zero dollars")]
    [InlineData("1", "one dollar")]
    [InlineData("25,1", "twenty-five dollars and ten cents")]
    [InlineData("0,01", "zero dollars and one cent")]
    [InlineData("45 100", "forty-five thousand one hundred dollars")]
    [InlineData("999 999 999,99",
        "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
    public void ConvertAmount_ReturnsExpectedWords_ForEnglish(string amount, string expectedWords)
    {
        var result = _converter.ConvertAmount(amount, Language.English);

        Assert.Equal(expectedWords, result);
    }

    [Theory]
    [InlineData("0", "null Dollar")]
    [InlineData("1", "ein Dollar")]
    [InlineData("25,1", "fünfundzwanzig Dollar und zehn Cent")]
    [InlineData("0,01", "null Dollar und ein Cent")]
    [InlineData("21", "einundzwanzig Dollar")]
    public void ConvertAmount_ReturnsExpectedWords_ForGerman(string amount, string expectedWords)
    {
        var result = _converter.ConvertAmount(amount, Language.German);

        Assert.Equal(expectedWords, result);
    }
}
