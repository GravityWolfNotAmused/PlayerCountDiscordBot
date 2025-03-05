using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordPlayerCountBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllImplementationsOf<TInterface>(this IServiceCollection services, bool isTransient = false)
        {
            var assemblies = new Assembly[] { Assembly.GetEntryAssembly()!, Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly() };

            var implementationTypes = assemblies.SelectMany(a => a.GetTypes())
                .Distinct()
                .Where(t => typeof(TInterface).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();

#if DEBUG
            Console.WriteLine($"Registering: {implementationTypes.Count} {typeof(TInterface).Name}");
#endif

            foreach (var type in implementationTypes)
            {
                if (isTransient)
                    services.AddTransient(typeof(TInterface), type);
                else
                    services.AddSingleton(typeof(TInterface), type);
            }

            return services;
        }
    }
}
