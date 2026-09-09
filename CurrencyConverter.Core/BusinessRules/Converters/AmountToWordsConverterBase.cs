using CurrencyConverter.Core.BusinessRules.Interfaces;
using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.BusinessRules.Converters;

public abstract class AmountToWordsConverterBase : IAmountToWordsConverter
{
    public abstract Language Language { get; }

    public string ConvertAmount(Money money)
    {
        var dollars = FormatComponent(money.WholeNumber, DollarUnitWord);

        if (money.Cents <= 0) return dollars;

        var cents = FormatComponent(money.Cents, CentUnitWord);
        return $"{dollars}{Connector}{cents}";
    }

    protected abstract string Connector { get; }

    protected abstract string NumberToWords(long value);

    protected abstract string DollarUnitWord(long value);

    protected abstract string CentUnitWord(long value);

    protected virtual string FormatNumberForUnit(long value) => NumberToWords(value);

    private string FormatComponent(long value, Func<long, string> unitWord) =>
        $"{FormatNumberForUnit(value)} {unitWord(value)}";
}
