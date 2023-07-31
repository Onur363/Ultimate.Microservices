FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Ultimate.SharedCommon/*csproj Shared/Ultimate.SharedCommon/
COPY Services/Order/Ultimate.Services.Order.Application/*.csproj Services/Order/Ultimate.Services.Order.Application/
COPY Services/Order/Ultimate.Services.Order.Domain/*.csproj Services/Order/Ultimate.Services.Order.Domain/
COPY Services/Order/Ultimate.Services.Order.Domain.Core/*.csproj Services/Order/Ultimate.Services.Order.Domain.Core/
COPY Services/Order/Ultimate.Services.Order.Infrastructure/*.csproj Services/Order/Ultimate.Services.Order.Infrastructure/
COPY Services/Order/Ultimate.Order.API/*.csproj Services/Order/Ultimate.Order.API/
RUN dotnet restore Services/Order/Ultimate.Order.API/*.csproj
COPY . .
RUN dotnet publish Services/Order/Ultimate.Order.API/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.Order.API.dll"]