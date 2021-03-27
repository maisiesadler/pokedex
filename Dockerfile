FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /build
COPY Pokedex.sln .
COPY src/Pokedex/Pokedex.csproj src/Pokedex/
COPY src/Pokedex.Tests.Unit/Pokedex.Tests.Unit.csproj src/Pokedex.Tests.Unit/
COPY src/Pokedex.Tests.Integration/Pokedex.Tests.Integration.csproj src/Pokedex.Tests.Integration/
ARG ASPNETCORE_ENVIRONMENT
ARG VERSION=1.0.0
RUN dotnet restore Pokedex.sln
COPY . .
RUN dotnet build -c Release /property:Version=$VERSION --no-restore && \
  dotnet test src/Pokedex.Tests.Unit/Pokedex.Tests.Unit.csproj -c Release --no-restore --no-build && \
  dotnet test src/Pokedex.Tests.Integration/Pokedex.Tests.Integration.csproj -c Release --no-restore --no-build && \
  dotnet publish src/Pokedex/Pokedex.csproj -c Release -o /build/publish --no-restore --no-build

FROM base AS final
WORKDIR /app
COPY --from=build /build/publish .
ENTRYPOINT ["dotnet", "Pokedex.dll"]
