FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Ultimate.SharedCommon/*csproj Shared/Ultimate.SharedCommon/
COPY Clients/Ultimate.WebCore/*.csproj Clients/Ultimate.WebCore/
RUN dotnet restore Clients/Ultimate.WebCore/*.csproj
COPY . .
RUN dotnet publish Clients/Ultimate.WebCore/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.WebCore.dll"]