FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Ultimate.SharedCommon/*csproj Shared/Ultimate.SharedCommon/
COPY Services/Basket/Ultimate.Basket/*.csproj Services/Basket/Ultimate.Basket/
RUN dotnet restore Services/Basket/Ultimate.Basket/*.csproj
COPY . .
RUN dotnet publish Services/Basket/Ultimate.Basket/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.Basket.dll"]