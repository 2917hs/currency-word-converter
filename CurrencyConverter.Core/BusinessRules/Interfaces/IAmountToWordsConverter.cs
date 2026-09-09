using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.BusinessRules.Interfaces;

public interface IAmountToWordsConverter
{
    Language Language { get; }

    string ConvertAmount(Money money);
}