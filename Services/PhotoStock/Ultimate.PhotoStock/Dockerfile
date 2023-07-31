FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Ultimate.SharedCommon/*csproj Shared/Ultimate.SharedCommon/
COPY Services/PhotoStock/Ultimate.PhotoStock/*.csproj Services/PhotoStock/Ultimate.PhotoStock/
RUN dotnet restore Services/PhotoStock/Ultimate.PhotoStock/*.csproj
COPY . .
RUN dotnet publish Services/PhotoStock/Ultimate.PhotoStock/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.PhotoStock.dll"]