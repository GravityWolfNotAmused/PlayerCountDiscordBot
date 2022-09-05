#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DiscordPlayerCountBot/DiscordPlayerCountBot.csproj", "DiscordPlayerCountBot/"]
RUN dotnet restore "DiscordPlayerCountBot/DiscordPlayerCountBot.csproj"
COPY . .
WORKDIR "/src/DiscordPlayerCountBot"
RUN dotnet build "DiscordPlayerCountBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiscordPlayerCountBot.csproj" -c Release -o /app/publish

FROM base AS final
ENV ISDOCKER true

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordPlayerCountBot.dll"]