using CurrencyConverter.Core.BusinessRules.Interfaces;
using CurrencyConverter.Core.Enumerations;
using CurrencyConverter.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Api.DTOs;

namespace CurrencyConverter.Api.Controllers;

[ApiController]
[Route("api/v1")]
public sealed class CurrencyConversionController(ICurrencyToWordsConverter converter) : ControllerBase
{
    [HttpGet("languages")]
    [ProducesResponseType(typeof(IEnumerable<LanguageOption>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<LanguageOption>> GetSupportedLanguages()
    {
        var options = LanguageCodes.All
            .Select(pair => new LanguageOption(pair.Value.Code, pair.Value.DisplayName));

        return Ok(options);
    }

    [HttpPost("currency/convert")]
    [ProducesResponseType(typeof(ConvertCurrencyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ConvertCurrencyResponse> Convert([FromBody] ConvertCurrencyRequest request)
    {
        if (!LanguageCodes.TryParse(request.Language, out var language))
        {
            var supported = string.Join(", ", LanguageCodes.SupportedCodes);
            throw new ValidationException(
                $"Unsupported language '{request.Language}'. Supported languages: {supported}.",
                nameof(request.Language));
        }

        var words = converter.ConvertAmount(request.Amount, language);
        return Ok(new ConvertCurrencyResponse(request.Amount, request.Language, words));
    }
}
