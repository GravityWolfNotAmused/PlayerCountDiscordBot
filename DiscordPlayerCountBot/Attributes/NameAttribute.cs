using System;

namespace DiscordPlayerCountBot.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        public string Name { get; private set; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
