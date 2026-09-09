namespace CurrencyConverter.Core.Enumerations;

public static class LanguageCodes
{
    private static readonly IReadOnlyDictionary<Language, LanguageMetadata> Metadata =
        new Dictionary<Language, LanguageMetadata>
        {
            [Language.English] = new("en", "English"),
            [Language.German] = new("de", "Deutsch (German)")
        };

    private static readonly IReadOnlyDictionary<string, Language> CodeToLanguage =
        Metadata.ToDictionary(pair => pair.Value.Code, pair => pair.Key, StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyDictionary<Language, LanguageMetadata> All => Metadata;

    public static IEnumerable<string> SupportedCodes => Metadata.Values.Select(m => m.Code);

    public static bool TryParse(string? code, out Language language)
    {
        if (code is not null && CodeToLanguage.TryGetValue(code, out var found))
        {
            language = found;
            return true;
        }

        language = default;
        return false;
    }
}
