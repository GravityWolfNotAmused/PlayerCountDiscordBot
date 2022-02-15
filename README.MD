### **Run the bot once for Config.json**
 
# Join My Discord Server

[![Discord Shield](https://discordapp.com/api/guilds/824404335105933382/widget.png?style=shield)](https://discord.gg/meBtwthmmD)

### Requirements:
[.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-3.1.13-windows-x64-installer)

### Notes:
**AntiVirus software may stop the executable from running.**

# Getting a Steam API key
In order to get a steam API key, you will need a domain to link it to. Trying looking into [Google's domain site](https://domains.google.com/registrar/). After you have gotten a domain navigate to [Steam'd Web API Key Register](https://steamcommunity.com/dev/apikey), and get your Steam API Key. Copy your `Key` and put it in your config file by replacing `SteamAPIKeyHere` with your Steam API Key.

![image](https://user-images.githubusercontent.com/24533882/103162963-c9304000-47b4-11eb-814e-c6156649b908.png)

![image](https://user-images.githubusercontent.com/24533882/149275986-27063a6e-f1b0-4c52-87f3-cf0a5f9fa77d.png)


# Creating a bot(s)
**You must repeat this step for each server you wish to track.**

First, navigate to [Discord's Developer Portal](https://discord.com/developers/applications), and create a new Application by selecting `New Application` in the top right of the page, and give you application a name:

![NewApplication](https://user-images.githubusercontent.com/24533882/103163050-da2d8100-47b5-11eb-99cd-8d6c92214f25.PNG)

After you have created an Application for your discord bot, you will need to navigate to the Bot tab, and click `Add Bot`:

![AddBot](https://user-images.githubusercontent.com/24533882/103163081-4f00bb00-47b6-11eb-9745-17210b6114dc.PNG)

After you have created your bot and named it, you will need to copy the bots `Token` into your config by replacing `DiscordTokenHere` with your bots token.

![image](https://user-images.githubusercontent.com/24533882/103163390-9ab56380-47ba-11eb-9c44-ec7f83343078.png)

![image](https://user-images.githubusercontent.com/24533882/149276159-a350543f-3091-4632-9597-7f24f362fce1.png)


# Configuring Server's
After you have added your Steam API key, and your bot's token you must add the `botAddress`, this address is your query address for the server. This is so we know what game server's player count to display in the status of each bot. In order to configure the bot to display the player count, you must add the IP, and the Port in the format `Address:Port` ex. `23.23.233.233:27015` where `127.0.0.1:2532` is located. This address is **NOT** the connection address.

![image](https://user-images.githubusercontent.com/24533882/149276263-edc7a259-9751-4125-84c9-e949cad7324a.png)

# Invite Your Bot
After all configuration steps have been taken, we must start the bot and invite them into our discord. The bot needs no special permissions due to it's responsibility being to update it's status. In order to invite your discord bot, you must replace `{clientID}` with your bots client ID in the URL below, this means you must also remove the curly braces too. If you do not know where to obtain your bot's client ID, navigate to `General Information` tab on the Discord Developer Portal the bot you wish, and copy your Client ID from there.

```
https://discord.com/oauth2/authorize?client_id={clientID}&scope=bot
```

### Additional Features:

- You may use `localhost` or `hostname` as your server address for the application to automatically resolve the hosts IP.
![image](https://user-images.githubusercontent.com/24533882/149276420-04551e2b-7516-437b-960c-f2493463325d.png)

- You may add the ChannelID variable to any bot to have the bot update the name of a text channel, or voice channel.
  - ChanelID does not need to be specified in either Docker, or Executable instances. (If you don't want it don't include it in configuration files.)
  - **Note:** Channel updates are rate limited to 2 requests per 10 mins. No matter how often your bot updates it's activity it will never update the channel as often.
  - This rate limit is enforced by Discord, and not the library used Discord.Net

![image](https://user-images.githubusercontent.com/24533882/153981476-4b258809-1860-4ec3-8454-bf826538bca7.png)

**Voice Channel Format:**

![image](https://user-images.githubusercontent.com/24533882/153981685-d84541e4-aa21-4731-a27d-448bb9a0332e.png)

**Text Channel Format:**

*Note: Text Channel names are limited to [a-z0-9-_]{2,100}*

![image](https://user-images.githubusercontent.com/24533882/153981690-130a1ca1-02ba-4d95-a5a0-4e98cd192443.png)

# Docker Support
[Get Docker](https://docs.docker.com/get-docker/)

## Version Tags

| Tag | Description |
| :----: | --- |
| latest | Latest stable releases |
| Beta | May be unstable and contain unfinished features |

## Usage

There are two ways to deploy bots with Docker.

### docker-compose (recommended)

 1. Create `docker-compose.yml` file 
 2. Copy into the file
  ```yaml
  version: "3.7"

  services:
    discord-bot:
       image: specker/discordplayercountbot
       environment:
         BOT_NAMES: "Bot1;Bot2"
         BOT_PUBADDRESSES: "127.0.0.1;127.0.0.1"
         BOT_PORTS: "16261;16262"
         BOT_DISCORD_TOKENS: "DiscordToken1;DiscordToken2"
         BOT_UPDATE_TIME: "30"
         BOT_STATUSES: "1;2"
         BOT_USENAMETAGS: "false;false"
         STEAM_API_KEY: "your steam api key"
  ```
 3. Edit variables and save the File
 4. Open terminal in a location of `docker-compose.yml` file and type `docker-compose up`
 5. Press Enter

### docker commandline

 1. Open Terminal
 2. Type:
  ```bash
  docker run \
    --name=PlayerCounterDiscordBot \
    -e BOT_NAMES='Bot1;Bot2' \
    -e BOT_PUBADDRESSES='127.0.0.1;127.0.0.1' \
    -e BOT_PORTS='16261;16262' \
    -e BOT_DISCORD_TOKENS='DiscordToken1;DiscordToken2' \
    -e BOT_UPDATE_TIME='30' \
    -e BOT_STATUSES='1;2' \
    -e BOT_USENAMETAGS='false;false' \
    -e STEAM_API_KEY='your steam api key' \
    specker/discordplayercountbot
  ```
 3. Edit values in single quotes and press enter

## Enviroment Variables

Container images are configured with help of those variables.

| Variable | Function |
| :----: | --- |
| `BOT_NAMES` | Names of bots |
| `BOT_PUBADDRESSES` | Addresses of gameservers |
| `BOT_PORTS` | Query ports of gameservers |
| `BOT_DISCORD_TOKENS` | Discord Bots tokens |
| `BOT_STATUSES` | Discord Bots statuses |
| `BOT_USENAMETAGS` | "TBD" |
| `STEAM_API_KEY` | Steam Api Key |
| `BOT_UPDATE_TIME` | How often bot should refresh |

To declare Multiple bots in docker separate values in `BOT_NAMES`, `BOT_PUBADDRESSES`, `BOT_PORTS`, `BOT_DISCORD_TOKENS`, `BOT_STATUSES` and `BOT_USENAMETAGS` with `;` 
 
 Example:
  
  One bot:
   `BOT_NAMES='Bot1'`
  
  Two bots:
   `BOT_NAMES='Bot1;Bot2'`

# Issues
All logging is tied behind _isDebug variable in the Config.json. Please report any issues to [Issues tab](https://github.com/GravityWolfNotAmused/PlayerCountDiscordBot/issues).

# Changes
Please fork this repository, and make a pull request for the changes you'd like to see to the project.
