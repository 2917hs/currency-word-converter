using System.Text;
using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.BusinessRules.Converters;

public sealed class GermanAmountToWordsConverter : AmountToWordsConverterBase
{
    private static readonly Dictionary<int, string> OnesTrailing = new()
    {
        [0] = "null", [1] = "eins", [2] = "zwei", [3] = "drei", [4] = "vier",
        [5] = "fünf", [6] = "sechs", [7] = "sieben", [8] = "acht", [9] = "neun",
        [10] = "zehn", [11] = "elf", [12] = "zwölf", [13] = "dreizehn", [14] = "vierzehn",
        [15] = "fünfzehn", [16] = "sechzehn", [17] = "siebzehn", [18] = "achtzehn", [19] = "neunzehn"
    };

    private static readonly Dictionary<int, string> OnesPrefix = new()
    {
        [1] = "ein", [2] = "zwei", [3] = "drei", [4] = "vier", [5] = "fünf",
        [6] = "sechs", [7] = "sieben", [8] = "acht", [9] = "neun"
    };

    private static readonly Dictionary<int, string> Tens = new()
    {
        [2] = "zwanzig", [3] = "dreißig", [4] = "vierzig", [5] = "fünfzig",
        [6] = "sechzig", [7] = "siebzig", [8] = "achtzig", [9] = "neunzig"
    };

    public override Language Language => Language.German;

    protected override string Connector => " und ";

    protected override string DollarUnitWord(long value) => "Dollar";

    protected override string CentUnitWord(long value) => "Cent";

    protected override string FormatNumberForUnit(long value) => value == 1 ? "ein" : NumberToWords(value);

    protected override string NumberToWords(long value)
    {
        if (value == 0) return "null";

        if (value == 1) return "eins";

        var millions = value / 1_000_000;
        var remainderAfterMillions = value % 1_000_000;
        var thousands = remainderAfterMillions / 1_000;
        var units = remainderAfterMillions % 1_000;

        var parts = new List<string>();

        if (millions > 0)
            parts.Add(millions == 1
                ? "eine Million"
                : $"{ThreeDigitGroupToWords((int)millions)} Millionen");

        var remainderUnderMillion = thousands * 1000 + units;
        if (remainderUnderMillion > 0 || parts.Count == 0)
        {
            var word = new StringBuilder();

            if (thousands > 0)
                word.Append(thousands == 1 ? "eintausend" : $"{ThreeDigitGroupToWords((int)thousands)}tausend");

            if (units > 0) word.Append(ThreeDigitGroupToWords((int)units));

            parts.Add(word.Length == 0 ? "null" : word.ToString());
        }

        return string.Join(' ', parts);
    }

    private static string ThreeDigitGroupToWords(int value)
    {
        var hundreds = value / 100;
        var remainder = value % 100;

        var builder = new StringBuilder();

        if (hundreds > 0) builder.Append(OnesPrefix[hundreds]).Append("hundert");

        if (remainder > 0) builder.Append(TwoDigitGroupToWords(remainder));

        return builder.ToString();
    }

    private static string TwoDigitGroupToWords(int value)
    {
        if (value < 20) return OnesTrailing[value];

        var tens = value / 10;
        var units = value % 10;

        return units == 0
            ? Tens[tens]
            : $"{OnesPrefix[units]}und{Tens[tens]}";
    }
}