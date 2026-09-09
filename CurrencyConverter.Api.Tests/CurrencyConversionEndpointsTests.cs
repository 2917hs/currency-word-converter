using System.Net;
using System.Net.Http.Json;
using CurrencyConverter.Api.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverter.Api.Tests;

public class CurrencyConversionEndpointsTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetLanguages_ReturnsEnglishAndGerman()
    {
        var response = await _client.GetAsync("/api/v1/languages");
        response.EnsureSuccessStatusCode();

        var languages = await response.Content.ReadFromJsonAsync<List<LanguageOption>>();

        Assert.NotNull(languages);
        Assert.Contains(languages, l => l.Code == "en");
        Assert.Contains(languages, l => l.Code == "de");
    }

    [Fact]
    public async Task Convert_ReturnsExpectedWords_ForValidAmount()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/currency/convert",
            new ConvertCurrencyRequest("25,1", "en"));
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ConvertCurrencyResponse>();

        Assert.NotNull(result);
        Assert.Equal("twenty-five dollars and ten cents", result.Words);
    }

    [Fact]
    public async Task Convert_ReturnsBadRequest_ForInvalidAmount()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/currency/convert",
            new ConvertCurrencyRequest("abc", "en"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Contains("Amount", problem.Errors.Keys);
    }
}
