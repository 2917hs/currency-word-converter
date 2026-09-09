namespace CurrencyToWordConverter.Core.Exceptions;

public sealed class CurrencyFormatException(string message) : Exception(message);