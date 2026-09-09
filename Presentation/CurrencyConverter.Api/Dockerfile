FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CurrencyConverter.Core/CurrencyConverter.Core.csproj CurrencyConverter.Core/
COPY Presentation/CurrencyConverter.Api/CurrencyConverter.Api.csproj Presentation/CurrencyConverter.Api/
RUN dotnet restore Presentation/CurrencyConverter.Api/CurrencyConverter.Api.csproj

COPY CurrencyConverter.Core/ CurrencyConverter.Core/
COPY Presentation/CurrencyConverter.Api/ Presentation/CurrencyConverter.Api/
RUN dotnet publish Presentation/CurrencyConverter.Api/CurrencyConverter.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CurrencyConverter.Api.dll"]
