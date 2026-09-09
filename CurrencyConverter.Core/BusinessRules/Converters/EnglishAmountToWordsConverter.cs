using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.BusinessRules.Converters;

public sealed class EnglishAmountToWordsConverter : AmountToWordsConverterBase
{
    private static readonly string[] Ones =
    {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"
    };

    private static readonly string[] Tens =
    {
        "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    };

    public override Language Language => Language.English;

    protected override string Connector => " and ";

    protected override string DollarUnitWord(long value) => value == 1 ? "dollar" : "dollars";

    protected override string CentUnitWord(long value) => value == 1 ? "cent" : "cents";

    protected override string NumberToWords(long value)
    {
        if (value == 0) return Ones[0];

        var millions = value / 1_000_000;
        var remainderAfterMillions = value % 1_000_000;
        var thousands = remainderAfterMillions / 1_000;
        var units = remainderAfterMillions % 1_000;

        var groups = new List<string>();

        if (millions > 0) groups.Add(ThreeDigitGroupToWords((int)millions) + " million");

        if (thousands > 0) groups.Add(ThreeDigitGroupToWords((int)thousands) + " thousand");

        if (units > 0 || groups.Count == 0) groups.Add(ThreeDigitGroupToWords((int)units));

        return string.Join(' ', groups);
    }

    private static string TwoDigitGroupToWords(int value)
    {
        if (value < 20) return Ones[value];

        var tens = value / 10;
        var units = value % 10;
        return units == 0 ? Tens[tens] : $"{Tens[tens]}-{Ones[units]}";
    }

    private static string ThreeDigitGroupToWords(int value)
    {
        var hundreds = value / 100;
        var remainder = value % 100;

        if (hundreds == 0) return TwoDigitGroupToWords(remainder);

        var hundredsWord = $"{Ones[hundreds]} hundred";
        return remainder == 0 ? hundredsWord : $"{hundredsWord} {TwoDigitGroupToWords(remainder)}";
    }
}