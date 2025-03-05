namespace DiscordPlayerCountBot.Exceptions;

internal class RconAuthenticationException(string? message) : Exception(message)
{
}