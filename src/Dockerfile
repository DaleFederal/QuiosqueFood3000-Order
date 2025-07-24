FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o arquivo da solução e os arquivos de projeto
COPY ["QuiosqueFood3000.Order.sln", "."]
COPY ["src/QuiosqueFood3000.Api/QuiosqueFood3000.Api.csproj", "src/QuiosqueFood3000.Api/"]
COPY ["src/QuiosqueFood3000.Application/QuiosqueFood3000.Application.csproj", "src/QuiosqueFood3000.Application/"]
COPY ["src/QuiosqueFood3000.Domain/QuiosqueFood3000.Domain.csproj", "src/QuiosqueFood3000.Domain/"]
COPY ["src/QuiosqueFood3000.Infraestructure/QuiosqueFood3000.Infraestructure.csproj", "src/QuiosqueFood3000.Infraestructure/"]

# Restaura as dependências para a solução inteira
RUN dotnet restore "QuiosqueFood3000.Order.sln"

# Copia o resto do código fonte
COPY . .

# Publica a aplicação
RUN dotnet publish "src/QuiosqueFood3000.Api/QuiosqueFood3000.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuiosqueFood3000.Api.dll"]