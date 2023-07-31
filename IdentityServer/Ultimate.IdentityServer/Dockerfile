FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Shared/Ultimate.SharedCommon/*csproj Shared/Ultimate.SharedCommon/
COPY IdentityServer/Ultimate.IdentityServer/*.csproj IdentityServer/Ultimate.IdentityServer/
RUN dotnet restore IdentityServer/Ultimate.IdentityServer/*.csproj
COPY . .
RUN dotnet publish IdentityServer/Ultimate.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Ultimate.IdentityServer.dll"]