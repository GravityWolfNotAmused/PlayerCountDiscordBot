using DiscordPlayerCountBot.EnvironmentParser.Base;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class EnvironmentParserResolver(IServiceProvider services)
{
    private readonly Dictionary<string, IEnvironmentParser> _environmentParsers = 
        services.GetServices<IEnvironmentParser>().ToDictionary(entry => entry.GetKey());

    public IEnvironmentParser GetEnvironmentParser(string variableName)
    {
        if (_environmentParsers.TryGetValue(variableName, out var parser))
        {
            return parser;
        }

        throw new KeyNotFoundException($"No parser found for environment variable: {variableName}");
    }

    public T ParseVariable<T>(string variableName)
    {
        var parser = (EnvironmentParserBase<T>) GetEnvironmentParser(variableName);

        return parser.ParseTyped(Environment.GetEnvironmentVariable(variableName));
    }
}
