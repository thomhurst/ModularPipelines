using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Automatically registers missing required dependencies for modules.
/// When a module has a required dependency (Optional = false) that is not registered,
/// this class will auto-register it so the pipeline can run successfully.
/// </summary>
internal static class ModuleAutoRegistrar
{
    /// <summary>
    /// Scans all registered modules and auto-registers any missing required dependencies.
    /// This is called during pipeline construction, before the host is built.
    /// </summary>
    /// <param name="services">The service collection containing module registrations.</param>
    public static void AutoRegisterMissingDependencies(IServiceCollection services)
    {
        var registeredModuleTypes = ServiceCollectionExtensions.GetRegisteredModuleTypes(services);
        var modulesToAdd = new HashSet<Type>();

        // Keep iterating until no new modules need to be added
        // (handles transitive dependencies)
        bool addedAny;
        do
        {
            addedAny = false;
            var allKnownTypes = registeredModuleTypes.Union(modulesToAdd).ToHashSet();

            foreach (var moduleType in allKnownTypes.ToList())
            {
                var dependencies = ModuleDependencyResolver.GetDependencies(moduleType);

                foreach (var (dependencyType, optional) in dependencies)
                {
                    // Skip optional dependencies - they don't need auto-registration
                    if (optional)
                    {
                        continue;
                    }

                    // Skip if already registered or queued for registration
                    if (allKnownTypes.Contains(dependencyType))
                    {
                        continue;
                    }

                    // Skip if not a valid module type
                    if (!IsValidModuleType(dependencyType))
                    {
                        continue;
                    }

                    modulesToAdd.Add(dependencyType);
                    addedAny = true;
                }
            }
        }
        while (addedAny);

        // Register all discovered missing dependencies using the proper registration method
        foreach (var moduleType in modulesToAdd)
        {
            // Track module type for consistency
            services.Configure<Options.ModuleRegistrationOptions>(opts => opts.RegisterModuleType(moduleType));

            // Use IModuleActivator for proper AsyncLocal context
            services.AddSingleton(typeof(IModule), sp =>
                sp.GetRequiredService<IModuleActivator>().CreateModule(moduleType, sp));
        }
    }

    /// <summary>
    /// Checks if a type is a valid module type that can be instantiated.
    /// </summary>
    private static bool IsValidModuleType(Type type)
    {
        return type.IsClass
               && !type.IsAbstract
               && !type.IsGenericTypeDefinition
               && type.IsAssignableTo(typeof(IModule));
    }
}
