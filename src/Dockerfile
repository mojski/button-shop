FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Directory.Build.props Directory.Build.props
COPY Directory.Packages.props Directory.Packages.props

COPY WebApi/ButtonShop.WebApi.csproj WebApi/ButtonShop.WebApi.csproj
COPY Application/ButtonShop.Application.csproj Application/ButtonShop.Application.csproj
COPY Domain/ButtonShop.Domain.csproj Domain/ButtonShop.Domain.csproj
COPY Infrastructure/ButtonShop.Infrastructure.csproj Infrastructure/ButtonShop.Infrastructure.csproj
COPY Common/ButtonShop.Common.csproj Common/ButtonShop.Common.csproj
#COPY ButtonShop.sln ButtonShop.sln
RUN dotnet restore WebApi/ButtonShop.WebApi.csproj

COPY . ./

RUN dotnet build WebApi/ButtonShop.WebApi.csproj --configuration Release --output /app/build
RUN dotnet publish WebApi/ButtonShop.WebApi.csproj --configuration Release --output /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_URLS=http://*:5080
EXPOSE 5080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ButtonShop.WebApi.dll"]
