FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . /app
RUN dotnet restore

WORKDIR /app

RUN ls -la /app
RUN ls -la ./
RUN dotnet publish /app/DiscordPlayerCountBot.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ISDOCKER=True
ENTRYPOINT ["dotnet", "DiscordPlayerCountBot.dll"]
