# Stage 1: Build Environment
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

# Copia o arquivo da solução para a raiz do WORKDIR
COPY QuiosqueFood3000.Order.sln .

# Copia os arquivos .csproj para restaurar as dependências, mantendo a estrutura de diretórios
COPY src/QuiosqueFood3000.Api/QuiosqueFood3000.Api.csproj                      src/QuiosqueFood3000.Api/
COPY src/QuiosqueFood3000.Application/QuiosqueFood3000.Application.csproj      src/QuiosqueFood3000.Application/
COPY src/QuiosqueFood3000.Domain/QuiosqueFood3000.Domain.csproj                src/QuiosqueFood3000.Domain/
COPY src/QuiosqueFood3000.Infraestructure/QuiosqueFood3000.Infraestructure.csproj src/QuiosqueFood3000.Infraestructure/
COPY QuiosqueFood3000.Order.UnitTests/QuiosqueFood3000.Order.UnitTests.csproj QuiosqueFood3000.Order.UnitTests/

# Restaura todos os pacotes NuGet para a solução
RUN dotnet restore QuiosqueFood3000.Order.sln

# Copia todo o resto do código fonte para o WORKDIR
COPY . .

# Publica a aplicação, especificando o projeto de saída
# A publicação compilará todo o código necessário
RUN dotnet publish "src/QuiosqueFood3000.Api/QuiosqueFood3000.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Final Production Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "QuiosqueFood3000.Api.dll"]
