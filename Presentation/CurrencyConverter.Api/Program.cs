using CurrencyConverter.Api.ErrorHandling;
using CurrencyConverter.Core.BusinessRules.Converters;
using CurrencyConverter.Core.BusinessRules.Interfaces;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

const string FrontendCorsPolicy = "FrontendCorsPolicy";
const string ConvertRateLimiterPolicy = "ConvertRateLimiterPolicy";

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        if (allowedOrigins.Length > 0)
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(ConvertRateLimiterPolicy, limiterOptions =>
    {
        limiterOptions.PermitLimit = 30;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAmountToWordsConverter, EnglishAmountToWordsConverter>();
builder.Services.AddSingleton<IAmountToWordsConverter, GermanAmountToWordsConverter>();
builder.Services.AddSingleton<ICurrencyToWordsConverter, CurrencyToWordsConverter>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseCors(FrontendCorsPolicy);

app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapHealthChecks("/health");
app.MapControllers().RequireRateLimiting(ConvertRateLimiterPolicy);

app.Run();
