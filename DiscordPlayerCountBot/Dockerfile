#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
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
ENV BOT_NAMES Bot1;Bot2
ENV BOT_PUBADDRESSES 127.0.0.1;127.0.0.1
ENV BOT_PORTS 16261;16262
ENV BOT_DISCORD_TOKENS DISCORDTOKEN1;DISCORDTOKEN2
ENV BOT_UPDATE_TIME 5
ENV STEAM_API_KEY SteamAPIkey
ENV BOT_STATUS 0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscordPlayerCountBot.dll"]