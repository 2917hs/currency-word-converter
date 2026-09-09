using CurrencyConverter.Core.BusinessRules.Interfaces;
using CurrencyConverter.Core.Enumerations;

namespace CurrencyConverter.Core.BusinessRules.Converters;

public class CurrencyToWordsConverter : ICurrencyToWordsConverter
{
    private readonly IReadOnlyDictionary<Language, IAmountToWordsConverter> _converters;

    public CurrencyToWordsConverter(IEnumerable<IAmountToWordsConverter> converters)
    {
        _converters = converters.ToDictionary(c => c.Language);
    }

    public string ConvertAmount(string rawAmount, Language language)
    {
        if (!_converters.TryGetValue(language, out var converter))
            throw new NotSupportedException($"No number-to-words converter is registered for '{language}'.");

        var amount = CurrencyAmountParser.Parse(rawAmount);
        return converter.ConvertAmount(amount);
    }
}