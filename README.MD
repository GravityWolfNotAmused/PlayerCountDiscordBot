### **Run the bot once for Config.json**
 
# Discord Server

![Discord Shield](https://discordapp.com/api/guilds/824404335105933382/widget.png?style=banner4)

### Requirements:
[.Net Core 3.1](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-desktop-3.1.13-windows-x64-installer)

### Notes:
**AntiVirus software may stop the executable from running.**

# Getting a Steam API key
In order to get a steam API key, you will need a domain to link it to. Trying looking into [Google's domain site](https://domains.google.com/registrar/). After you have gotten a domain navigate to [Steam'd Web API Key Register](https://steamcommunity.com/dev/apikey), and get your Steam API Key. Copy your `Key` and put it in your config file by replacing `SteamAPIKeyHere` with your Steam API Key.

![image](https://user-images.githubusercontent.com/24533882/103162963-c9304000-47b4-11eb-814e-c6156649b908.png)

![image](https://user-images.githubusercontent.com/24533882/103163310-cd129100-47b9-11eb-955a-d7a334159e19.png)

# Creating a bot(s)
**You must repeat this step for each server you wish to track.**

First, navigate to [Discord's Developer Portal](https://discord.com/developers/applications), and create a new Application by selecting `New Application` in the top right of the page, and give you application a name:

![NewApplication](https://user-images.githubusercontent.com/24533882/103163050-da2d8100-47b5-11eb-99cd-8d6c92214f25.PNG)

After you have created an Application for your discord bot, you will need to navigate to the Bot tab, and click `Add Bot`:

![AddBot](https://user-images.githubusercontent.com/24533882/103163081-4f00bb00-47b6-11eb-9745-17210b6114dc.PNG)

After you have created your bot and named it, you will need to copy the bots `Token` into your config by replacing `DiscordTokenHere` with your bots token.

![image](https://user-images.githubusercontent.com/24533882/103163390-9ab56380-47ba-11eb-9c44-ec7f83343078.png)

![image](https://user-images.githubusercontent.com/24533882/103163287-9b99c580-47b9-11eb-82d6-296d00dcf6fd.png)


# Configuring Server's
After you have added your Steam API key, and your bot's token you must add the `botAddress`, this address is your query address for the server. This is so we know what game server's player count to display in the status of each bot. In order to configure the bot to display the player count, you must add the IP, and the Port in the format `Address:Port` ex. `23.23.233.233:27015` where `127.0.0.1:2532` is located. This address is **NOT** the connection address.

![image](https://user-images.githubusercontent.com/24533882/103163337-14008680-47ba-11eb-86eb-94c8f66d49ff.png)

# Invite Your Bot
After all configuration steps have been taken, we must start the bot and invite them into our discord. The bot needs no special permissions due to it's responsibility being to update it's status. In order to invite your discord bot, you must replace `{clientID}` with your bots client ID in the URL below, this means you must also remove the curly braces too. If you do not know where to obtain your bot's client ID, navigate to `General Information` tab on the Discord Developer Portal the bot you wish, and copy your Client ID from there.

```
https://discord.com/oauth2/authorize?client_id={clientID}&scope=bot
```

# Issues
All logging is tied behind _isDebug variable in the Config.json. Please report any issues to [Issues tab](https://github.com/GravityWolfNotAmused/PlayerCountDiscordBot/issues).

# Changes
Please fork this repository, and make a pull request for the changes you'd like to see to the project.

