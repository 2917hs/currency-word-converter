using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.BusinessRules.Interfaces;

public interface ICurrencyToWordsConverter
{
    string ConvertAmount(string amount, Language language);
}