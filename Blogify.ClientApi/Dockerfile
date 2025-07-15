FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blogify.ClientApi/Blogify.ClientApi.csproj", "Blogify.ClientApi/"]
RUN dotnet restore "Blogify.ClientApi/Blogify.ClientApi.csproj"
COPY . .
WORKDIR "/src/Blogify.ClientApi"
RUN dotnet build "./Blogify.ClientApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blogify.ClientApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blogify.ClientApi.dll"]
