FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuiosqueFood3000.Api/QuiosqueFood3000.Api.csproj", "QuiosqueFood3000.Api/"]
COPY ["QuiosqueFood3000.Infraestructure/QuiosqueFood3000.Infraestructure.csproj", "QuiosqueFood3000.Infraestructure/"]
COPY ["QuiosqueFood3000.Application/QuiosqueFood3000.Application.csproj", "QuiosqueFood3000.Application/"]
COPY ["QuiosqueFood3000.Domain/QuiosqueFood3000.Domain.csproj", "QuiosqueFood3000.Domain/"]
RUN dotnet restore "QuiosqueFood3000.Api/QuiosqueFood3000.Api.csproj"
COPY . .
WORKDIR "/src/QuiosqueFood3000.Api"
RUN dotnet build "QuiosqueFood3000.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "QuiosqueFood3000.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuiosqueFood3000.Api.dll"]
