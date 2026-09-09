namespace CurrencyConverter.Core.Enumerations;

public enum Language
{
    English,
    German
}

public static class LanguageCodes
{
    private const string English = "en";

    private const string German = "de";

    public static readonly IReadOnlyDictionary<string, Language> CodeToLanguage =
        new Dictionary<string, Language>(StringComparer.OrdinalIgnoreCase)
        {
            [English] = Language.English,
            [German] = Language.German
        };

    public static readonly IReadOnlyDictionary<Language, string> LanguageToCode =
        new Dictionary<Language, string>
        {
            [Language.English] = English,
            [Language.German] = German
        };

    public static readonly IReadOnlyDictionary<Language, string> LanguageToDisplayName =
        new Dictionary<Language, string>
        {
            [Language.English] = "English",
            [Language.German] = "Deutsch (German)"
        };

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