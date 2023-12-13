#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BosonHiggs/BosonHiggsApi.csproj", "BosonHiggs/BosonHiggsApi.csproj"]
RUN ls -lah
RUN dotnet restore "BosonHiggs/BosonHiggsApi.csproj"
COPY . .
RUN ls -lah
WORKDIR "/src/BosonHiggs"
RUN dotnet build "BosonHiggsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BosonHiggsApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BosonHiggsApi.dll"]
