namespace CurrencyConverter.Core.Exceptions;

public sealed class ValidationException(string message, string fieldName = "Amount") : Exception(message)
{
    public string FieldName { get; } = fieldName;
}
