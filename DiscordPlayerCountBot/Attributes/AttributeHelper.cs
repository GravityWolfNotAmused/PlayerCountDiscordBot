using System.Linq;

namespace DiscordPlayerCountBot.Attributes
{
    public static class AttributeHelper
    {
        public static string GetNameFromAttribute(object obj)
        {
            var nameAttribute = obj.GetType().GetCustomAttributes(true).Where(attribute => attribute.GetType() == typeof(NameAttribute)).Cast<NameAttribute>().FirstOrDefault();
            var label = nameAttribute?.Name ?? obj.GetType().Name;
            return label;
        }
    }
}
