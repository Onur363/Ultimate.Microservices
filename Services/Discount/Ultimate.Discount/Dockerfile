FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Ultimate.SharedCommon/*csproj Shared/Ultimate.SharedCommon/
COPY Services/Discount/Ultimate.Discount/*.csproj Services/Discount/Ultimate.Discount/
RUN dotnet restore Services/Discount/Ultimate.Discount/*.csproj
COPY . .
RUN dotnet publish Services/Discount/Ultimate.Discount/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.Discount.dll"]