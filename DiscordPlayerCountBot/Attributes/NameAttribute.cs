namespace DiscordPlayerCountBot.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class NameAttribute(string name) : Attribute
{
    public string Name { get; private set; } = name;
}