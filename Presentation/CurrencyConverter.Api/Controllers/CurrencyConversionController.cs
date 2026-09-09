using CurrencyConverter.Core.BusinessRules.Interfaces;
using CurrencyConverter.Core.Enumerations;
using CurrencyConverter.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Api.DTOs;

namespace CurrencyConverter.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class CurrencyConversionController(ICurrencyToWordsConverter converter) : ControllerBase
{
    [HttpGet("languages")]
    [ProducesResponseType(typeof(IEnumerable<LanguageOption>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<LanguageOption>> GetSupportedLanguages()
    {
        var options = LanguageCodes.LanguageToCode
            .Select(pair => new LanguageOption(pair.Value, LanguageCodes.LanguageToDisplayName[pair.Key]));

        return Ok(options);
    }

    [HttpPost("currency/convert")]
    [ProducesResponseType(typeof(ConvertCurrencyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ConvertCurrencyResponse> Convert([FromBody] ConvertCurrencyRequest request)
    {
        if (!LanguageCodes.TryParse(request.Language, out var language))
        {
            var supported = string.Join(", ", LanguageCodes.CodeToLanguage.Keys);
            ModelState.AddModelError(nameof(request.Language),
                $"Unsupported language '{request.Language}'. Supported languages: {supported}.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var words = converter.ConvertAmount(request.Amount, language);
            return Ok(new ConvertCurrencyResponse(request.Amount, request.Language, words));
        }
        catch (CurrencyFormatException ex)
        {
            ModelState.AddModelError(nameof(request.Amount), ex.Message);
            return ValidationProblem(ModelState);
        }
    }
}