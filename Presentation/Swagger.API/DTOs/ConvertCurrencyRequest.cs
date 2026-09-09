using System.ComponentModel.DataAnnotations;

namespace Swagger.API.DTOs;

public sealed record ConvertCurrencyRequest(
    [Required] string Amount,
    [Required] string Language);