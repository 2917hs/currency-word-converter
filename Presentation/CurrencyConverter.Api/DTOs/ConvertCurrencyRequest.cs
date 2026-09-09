using System.ComponentModel.DataAnnotations;

namespace CurrencyConverter.Api.DTOs;

public sealed record ConvertCurrencyRequest(
    [Required] string Amount,
    [Required] string Language);