using CurrencyToWordConverter.Core.Enumerations;

namespace CurrencyToWordConverter.Core.Interfaces;

public interface IAmountToWordsConverter
{
    Language Language { get; }

    string ConvertAmount(Money amount);
}