FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /
COPY ["src/Presentation/Presentation.csproj", "src/Presentation/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Presentation/Presentation.csproj"
COPY . .
WORKDIR "/src/Presentation"
RUN dotnet build "Presentation.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll", "--environment", "Development"]