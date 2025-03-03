FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . ./
RUN dotnet restore

WORKDIR /app

RUN dotnet publish DiscordPlayerCountBot.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ISDOCKER=True
ENTRYPOINT ["dotnet", "DiscordPlayerCountBot.dll"]
