FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# 讓 ASP.NET Core 監聽 8080/8081
ENV ASPNETCORE_URLS=http://+:80;
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blogify.AdminApi/Blogify.AdminApi.csproj", "Blogify.AdminApi/"]
RUN dotnet restore "Blogify.AdminApi/Blogify.AdminApi.csproj"
COPY . .
WORKDIR "/src/Blogify.AdminApi"
RUN dotnet build "./Blogify.AdminApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blogify.AdminApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Blogify.AdminApi.dll"]
