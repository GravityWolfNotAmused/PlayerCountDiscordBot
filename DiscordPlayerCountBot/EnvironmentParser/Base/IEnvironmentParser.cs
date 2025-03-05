namespace DiscordPlayerCountBot.EnvironmentParser.Base;

public interface IEnvironmentParser
{
    object Parse(string? environmentVariable);
    string GetKey();
}