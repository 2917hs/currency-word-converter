using CurrencyToWordConverter.Core.Enumerations;

namespace CurrencyToWordConverter.Core.Interfaces;

public interface ICurrencyToWordsConverter
{
    string ConvertAmount(Money amount, Language language = Language.English);
}