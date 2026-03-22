FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SoccerPro.API/SoccerPro.API.csproj", "SoccerPro.API/"]
COPY ["SoccerPro.Application/SoccerPro.Application.csproj", "SoccerPro.Application/"]
COPY ["SoccerPro.Domain/SoccerPro.Domain.csproj", "SoccerPro.Domain/"]
COPY ["SoccerPro.Infrastructure/SoccerPro.Infrastructure.csproj", "SoccerPro.Infrastructure/"]
RUN dotnet restore "SoccerPro.API/SoccerPro.API.csproj"
COPY . .
WORKDIR "/src/SoccerPro.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SoccerPro.API.dll"]
