using System.Globalization;
using CurrencyToWordConverter.Core.Exceptions;

namespace CurrencyToWordConverter.Core.BusinessRules;

public class CurrencyAmountParser
{
    public static Money Parse(string? rawInput)
    {
        if (string.IsNullOrWhiteSpace(rawInput)) throw new CurrencyFormatException("Amount must not be empty.");

        var trimmed = rawInput.Trim();

        var commaIndex = trimmed.IndexOf(',');
        var dollarsPart = commaIndex >= 0 ? trimmed[..commaIndex] : trimmed;
        var centsPart = commaIndex >= 0 ? trimmed[(commaIndex + 1)..] : null;

        if (centsPart is not null && centsPart.IndexOf(',') >= 0)
            throw new CurrencyFormatException("Amount must contain at most one ',' separator.");

        var dollars = ParseDollars(dollarsPart);
        var cents = ParseCents(centsPart);

        return new Money(cents, dollars);
    }

    private static long ParseDollars(string dollarsPart)
    {
        var digitsOnly = dollarsPart.Replace(" ", string.Empty).Replace("\u00A0", string.Empty);

        if (digitsOnly.Length == 0) throw new CurrencyFormatException("Amount must include a whole-dollar value.");

        if (!IsAllAsciiDigits(digitsOnly))
            throw new CurrencyFormatException(
                $"'{dollarsPart}' is not a valid whole-dollar amount. Only digits and spaces are allowed.");

        if (!long.TryParse(digitsOnly, NumberStyles.None, CultureInfo.InvariantCulture, out var dollars))
            throw new CurrencyFormatException($"'{dollarsPart}' is too large to convert.");

        if (dollars > Money.MaxWholeNumber)
            throw new CurrencyFormatException(
                $"The maximum supported amount is {Money.MaxWholeNumber.ToString("N0", CultureInfo.InvariantCulture)} dollars.");

        return dollars;
    }

    private static int ParseCents(string? centsPart)
    {
        if (centsPart is null) return 0;

        if (centsPart.Length is 0 or > 2 || !IsAllAsciiDigits(centsPart))
            throw new CurrencyFormatException(
                "Cents must be given as one or two digits after the ',' (e.g. '25,1' or '25,10').");

        return centsPart.Length == 1
            ? int.Parse(centsPart, CultureInfo.InvariantCulture) * 10
            : int.Parse(centsPart, CultureInfo.InvariantCulture);
    }

    private static bool IsAllAsciiDigits(string value)
    {
        foreach (var c in value)
            if (c is < '0' or > '9')
                return false;

        return true;
    }
}