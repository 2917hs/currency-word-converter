using CurrencyToWordConverter.Core.BusinessRules.Converters;
using CurrencyToWordConverter.Core.BusinessRules.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAmountToWordsConverter, EnglishAmountToWordsConverter>();
builder.Services.AddSingleton<IAmountToWordsConverter, GermanAmountToWordsConverter>();
builder.Services.AddSingleton<ICurrencyToWordsConverter, CurrencyToWordsConverter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();