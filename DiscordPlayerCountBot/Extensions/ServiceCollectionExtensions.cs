using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordPlayerCountBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllImplementationsOf<TInterface>(this IServiceCollection services, bool isTransient = false, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                assemblies = [typeof(TInterface).Assembly];

            var implementationTypes = assemblies.SelectMany(a => a.GetTypes())
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