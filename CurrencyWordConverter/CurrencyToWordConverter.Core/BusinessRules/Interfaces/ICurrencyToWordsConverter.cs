using CurrencyToWordConverter.Core.Enumerations;

namespace CurrencyToWordConverter.Core.BusinessRules.Interfaces;

public interface ICurrencyToWordsConverter
{
    string ConvertAmount(string amount, Language language = Language.English);
}