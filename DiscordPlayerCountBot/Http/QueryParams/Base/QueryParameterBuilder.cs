namespace PlayerCountBot.Http
{
    public class QueryParameterBuilder : IQueryParameterBuilder
    {
        public virtual string CreateQueryParameterString()
        {
            var type = GetType();
            var properties = type.GetProperties();
            var queryString = string.Empty;
            var isFirstParameter = true;

            foreach (var property in properties.Reverse())
            {
                var value = property.GetValue(this);

                if (value == null) continue;

                if (!isFirstParameter)
                {
                    queryString += "&";
                }

                if (isFirstParameter)
                {
                    queryString += "?";
                    isFirstParameter = false;
                }

                if (value != null)
                {
                    var nameAttribute = property.CustomAttributes.Where(attribute => attribute.AttributeType == typeof(NameAttribute)).FirstOrDefault();

                    if (nameAttribute != null)
                    {
                        queryString += $"{nameAttribute.ConstructorArguments[0].Value?.ToString()}={value}";
                        continue;
                    }

                    queryString += $"{property.Name}={value}";
                }
            }

            return queryString;
        }
    }
}