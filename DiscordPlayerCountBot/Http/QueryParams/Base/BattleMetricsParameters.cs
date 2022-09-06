using DiscordPlayerCountBot.Attributes;
using System;
using System.Linq;

namespace DiscordPlayerCountBot.Http
{

    public class BattleMetricsParameters : QueryParameterBuilder
    {
        public override string CreateQueryParameterString()
        {
            var filterString = "";
            var type = GetType();

            int i = 0;
            type.GetProperties().ToList().ForEach(property =>
            {
                var propertyValue = property.GetValue(this);

                if (propertyValue != null)
                {
                    if (Attribute.GetCustomAttribute(property, typeof(NameAttribute)) is not NameAttribute nameAttribute)
                    {
                        throw new Exception("TODO: Throw actual error, not generic exception. Due to coding standards, we need attributes on parameters.");
                    }

                    if (i == 0)
                        filterString += "?";

                    if (i > 0)
                        filterString += "&";

                    filterString += $"filter[{nameAttribute.Name}]={propertyValue}";
                    i++;
                }
            });

            return filterString;
        }
    }
}
