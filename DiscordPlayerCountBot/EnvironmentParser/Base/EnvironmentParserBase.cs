namespace DiscordPlayerCountBot.EnvironmentParser.Base;

public abstract class EnvironmentParserBase<T> : IEnvironmentParser
{
    public abstract string GetKey();
    public object Parse(string? environmentVariable) => ParseTyped(environmentVariable) ?? throw new Exception($"{GetType().Name} ({GetKey()} could not parse: {environmentVariable}");
    public abstract T ParseTyped(string? environmentVariable);
}