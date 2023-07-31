FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Gateways/Ultimate.Gateway/*.csproj Gateways/Ultimate.Gateway/
RUN dotnet restore Gateways/Ultimate.Gateway/*.csproj
COPY . .
RUN dotnet publish Gateways/Ultimate.Gateway/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.Gateway.dll"]