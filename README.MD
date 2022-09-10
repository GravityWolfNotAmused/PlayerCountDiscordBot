#### Need help? Have suggestions? Join My Discord Server! 

![GitHub release (latest by date)](https://img.shields.io/github/v/release/GravityWolfNotAmused/PlayerCountDiscordBot)

[![Discord Shield](https://discordapp.com/api/guilds/824404335105933382/widget.png?style=shield)](https://discord.gg/meBtwthmmD)

#### Want to Download? [Releases](https://github.com/GravityWolfNotAmused/PlayerCountDiscordBot/releases)
#### Requirements: [.Net Core 6](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-6.0.8-windows-x64-installer)

# Generating a Config
After installation of .Net Runtime 6, you need to run the application one time. The application will state to you that it has created a default config.json file. Exit the application, and configure the file.

![image](https://user-images.githubusercontent.com/24533882/188336821-4d368b36-ecfb-44a5-88f1-b05909f20b05.png)

# Getting a Steam API Key
In order to get a Steam API key, you will need a domain to link it to. Trying looking into [Google's domain site](https://domains.google.com/registrar/). After you have gotten a domain navigate to [Steam'd Web API Key Register](https://steamcommunity.com/dev/apikey), and get your Steam API Key. Copy your `Key` and put it in your config file by replacing `SteamAPIKeyHere` with your Steam API Key.

![image](https://user-images.githubusercontent.com/24533882/103162963-c9304000-47b4-11eb-814e-c6156649b908.png)

![image](https://user-images.githubusercontent.com/24533882/188550022-53db7967-ba8d-4100-b258-c81dc67514ca.png)

# Getting a Battle Metrics Key
In order to get a Battle Metrics API Key, you will need to visit [here](https://www.battlemetrics.com/developers), and generate an api token. After you must replace `Here` with your Battle Metrics API Key.

![image](https://user-images.githubusercontent.com/24533882/188550180-070db933-0db9-45bd-bd17-caaf03ffc00c.png)


# Creating a bot(s)
**You must repeat this step for each server you wish to track.**

First, navigate to [Discord's Developer Portal](https://discord.com/developers/applications), and create a new Application by selecting `New Application` in the top right of the page, and give you application a name:

![NewApplication](https://user-images.githubusercontent.com/24533882/103163050-da2d8100-47b5-11eb-99cd-8d6c92214f25.PNG)

After you have created an Application for your discord bot, you will need to navigate to the Bot tab, and click `Add Bot`:

![AddBot](https://user-images.githubusercontent.com/24533882/103163081-4f00bb00-47b6-11eb-9745-17210b6114dc.PNG)

After you have created your bot and named it, you will need to copy the bots `Token` into your config by replacing `DiscordTokenHere` with your bots token.

![image](https://user-images.githubusercontent.com/24533882/103163390-9ab56380-47ba-11eb-9c44-ec7f83343078.png)

![image](https://user-images.githubusercontent.com/24533882/188548745-78d978f5-652d-4a54-9fde-b490370ebc4f.png)


# Configuring Server's
After you have added your keyS, and your bot's token you must add the `botAddress`, this address is IP and port for the server. **Which port? That depends on your provider. Look [here.](#data-providers)**

If your data provider depends on a port, your address must be formatted as: `address:port`. Example: `127.0.0.1:3450`

![image](https://user-images.githubusercontent.com/24533882/188548793-322db8f4-10d1-4963-9b88-16f9b7870b64.png)

# Invite Your Bot
After all configuration steps have been taken, we must start the bot and invite them into our discord. The bot needs no special permissions due to it's responsibility being to update it's status. In order to invite your discord bot, you must replace `{clientID}` with your bots client ID in the URL below, this means you must also remove the curly braces too. If you do not know where to obtain your bot's client ID, navigate to `General Information` tab on the Discord Developer Portal the bot you wish, and copy your Client ID from there.

```
https://discord.com/oauth2/authorize?client_id={clientID}&scope=bot
```

# Additional Features:

#### Activity
Discord allows users to be under specific activities states. There is no guarentee that these activity type will display the same as a non-bot user performing that activity.

The available states that I have are as follows:
| Name | Config Value | Description |
| :----: | --- | --- |
| Playing | 0 | The user is playing a game. | 
| Streaming | 1 | The user is streaming online |
| Listening | 2 | The user is listening to a song. |
| Watching | 3 | The user is watching some form of media. |
| CustomStatus | 4 | The user has set a customer status. **Do not use.** |
| Competing | 5 | The user is competing in a game. |

#### Data Providers
This bot works with many different providers to display your game server's player counts. The available providers are listed below.


| Name | Config Value | Note | Port Type |
| :----: | --- | --- | ---- |
| Steam | 0 | Limit 1 Milion requests per 24 hours | Steam Query Port |
| CFX | 1 | N/A | Game Port |
| Scum | 2 | N/A | Game Port |
| Minecraft | 3 | N/A | Game Port |
| BattleMetrics | 4 | Uses Battle Metrics ID. [ID is the number in this URL](https://www.battlemetrics.com/servers/minecraft/5873087) | N/A |

#### Address Resolution
You may use `localhost` or `hostname` as your server address for the application to automatically resolve the hosts IP.

![image](https://user-images.githubusercontent.com/24533882/188549477-753fb698-0d34-479b-970e-b2e265e90bd8.png)

#### Channel Player Counts
You may add the ChannelID variable to any bot to have the bot update the name of a text channel, or voice channel.
ChanelID does not need to be specified in either Docker, or Executable instances. (If you don't want it don't include it in configuration files or set it to null.)

**Note:** Channel updates are rate limited to 2 requests per 10 mins. No matter how often your bot updates it's activity it will never update the channel as often.

This rate limit is enforced by Discord, and not the library used Discord.Net

![image](https://user-images.githubusercontent.com/24533882/188549536-35000ab2-5f90-416c-b641-29a534c9511f.png)

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
| dev | May be unstable and contain unfinished features |

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
         BOT_PROVIDERTYPES: "0;0"
         BOT_APPLICATION_VARIABLES: "SteamAPIKey,Here;BattleMetricsKey,Here"
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
    -e BOT_PROVIDERTYPES='0;0' \
    -e BOT_USENAMETAGS='false;false' \
    -e BOT_APPLICATION_VARIABLES='SteamAPIKey,Here;BattleMetricsKey,Here' \
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
| `BOT_PROVIDERTYPES` | Who provides the data for the bot. |
| `BOT_UPDATE_TIME` | How often bot should refresh |
| `BOT_APPLICATION_VARIABLES` | Application Variables for all bots |

To declare Multiple bots in docker separate values in `BOT_NAMES`, `BOT_PUBADDRESSES`, `BOT_PORTS`, `BOT_DISCORD_TOKENS`, `BOT_STATUSES`, `BOT_PROVIDERTYPES`, and `BOT_USENAMETAGS` with `;` 
 
 Example:
  
  One bot:
   `BOT_NAMES='Bot1'`
  
  Two bots:
   `BOT_NAMES='Bot1;Bot2'`

`BOT_APPLICATION_VARIABLES` is a key value pair. The key and value are seperated by commas, and each pair is seperated by a semi-colon.

 Example:
  `BOT_APPLICATION_VARIABLES='SteamAPIKey,34124124123rfwer1424124124;BattleMetricsKey,24124124124rwefwrgery5323423412312'`
  
# Issues
Please report any issues to [Issues tab](https://github.com/GravityWolfNotAmused/PlayerCountDiscordBot/issues) or my discord.

# Changes
Please fork this repository, and make a pull request for the changes you'd like to see to the project.
