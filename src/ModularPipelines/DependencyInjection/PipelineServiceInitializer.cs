using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Runs the <see cref="IInitializer"/> services registered in a pipeline container.
/// </summary>
/// <remarks>
/// Mirrors <c>Initialization.Microsoft.Extensions.DependencyInjection</c>'s <c>InitializeAsync</c>
/// step for step: instance and type initializers are resolved first, then each factory registration
/// is checked against the currently loaded types and resolved before the next one is checked.
/// The only difference is that each assembly's <see cref="IInitializer"/> types are cached, so a
/// factory check no longer enumerates every loaded type. The library's per-factory scan made every
/// pipeline build CPU-bound in proportion to (factory registrations x loaded types).
/// </remarks>
internal static class PipelineServiceInitializer
{
    private static readonly LoadedInitializerTypes SharedLoadedInitializerTypes = new(FindInitializerTypes);

    public static Task InitializeAsync(IServiceProvider serviceProvider) =>
        InitializeAsync(serviceProvider, SharedLoadedInitializerTypes);

    internal static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        LoadedInitializerTypes loadedInitializerTypes)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(loadedInitializerTypes);

        // Keyed descriptors throw on ImplementationType access and can never be resolved by
        // service type alone, so they cannot be initializers here.
        var descriptors = serviceProvider
            .GetRequiredService<IPipelineServiceContainerWrapper>()
            .ServiceCollection
            .Where(descriptor => !descriptor.IsKeyedService)
            .ToList();

        var initializers = descriptors
            .Where(descriptor => descriptor.ImplementationInstance is IInitializer)
            .Concat(descriptors.Where(descriptor => descriptor.ImplementationType is { } implementationType
                                                    && IsInitializer(implementationType)))
            .Select(descriptor => GetService(serviceProvider, descriptor))
            .OfType<IInitializer>()
            .ToList();

        // As in the library, each factory is checked and then resolved before the next check, so
        // assemblies loaded while resolving earlier services are visible to later checks.
        var dynamicAssemblyTypes = new Dictionary<Assembly, Type[]>();
        foreach (var descriptor in descriptors)
        {
            if (descriptor.ImplementationFactory is not { } factory
                || !CanProduceInitializer(descriptor.ServiceType, factory.Method.ReturnType, loadedInitializerTypes, dynamicAssemblyTypes))
            {
                continue;
            }

            var service = GetService(serviceProvider, descriptor);

            // Resolving runs user code, which can emit new types into dynamic assemblies.
            dynamicAssemblyTypes.Clear();

            if (service is IInitializer initializer)
            {
                initializers.Add(initializer);
            }
        }

        // A singleton forwarded through several service types resolves to the same instance for
        // each registration; initialize it once, in the order it was first discovered.
        var distinctInitializers = initializers.Distinct<IInitializer>(ReferenceEqualityComparer.Instance);

        foreach (var batch in distinctInitializers.GroupBy(initializer => initializer.Order).OrderBy(batch => batch.Key))
        {
            // Start every initializer in the batch before awaiting, so a synchronous throw
            // cannot leave already-started initializers running unobserved.
            await Task.WhenAll([.. batch.Select(StartInitializer)]).ConfigureAwait(false);
        }
    }

    private static object? GetService(IServiceProvider serviceProvider, ServiceDescriptor descriptor)
    {
        if (descriptor.Lifetime != ServiceLifetime.Singleton)
        {
            var implementationType = descriptor.ImplementationType
                                     ?? descriptor.ImplementationInstance?.GetType()
                                     ?? descriptor.ServiceType;
            throw new InvalidOperationException(
                $"Service Provider Initializers are only supported for Singletons. {implementationType.Name} is {descriptor.Lifetime}");
        }

        return serviceProvider.GetService(descriptor.ServiceType);
    }

    private static Task StartInitializer(IInitializer initializer)
    {
        try
        {
            return initializer.InitializeAsync();
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }

    private static bool CanProduceInitializer(
        Type serviceType,
        Type factoryReturnType,
        LoadedInitializerTypes loadedInitializerTypes,
        Dictionary<Assembly, Type[]> dynamicAssemblyTypes)
    {
        return IsInitializer(factoryReturnType)
               || loadedInitializerTypes.Get(dynamicAssemblyTypes).Any(type =>
                   serviceType.IsAssignableFrom(type) && factoryReturnType.IsAssignableFrom(type));
    }

    private static bool IsInitializer(Type type) => typeof(IInitializer).IsAssignableFrom(type);

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Matches the Initialization library's loaded-type scan. A factory can only return a type the trimmer kept, so types removed by trimming cannot affect which factories produce initializers.")]
    private static Type[] FindInitializerTypes(Assembly assembly) =>
        [.. AssemblyTypeLoader.GetLoadableTypes(assembly).Where(IsInitializer)];

    /// <summary>
    /// Lists the loaded <see cref="IInitializer"/> types, reading each non-dynamic assembly's types once
    /// per process and each dynamic assembly's types once per initialization.
    /// </summary>
    internal sealed class LoadedInitializerTypes(Func<Assembly, Type[]> findInitializerTypes)
    {
        private readonly ConditionalWeakTable<Assembly, Lazy<Type[]>> _typesByAssembly = [];

        /// <param name="dynamicAssemblyTypes">
        /// Per-initialization cache for dynamic assemblies (for example mocking proxies), which can
        /// gain types at any time and so cannot be cached for the whole process. The caller clears it
        /// after resolving a factory.
        /// </param>
        public IEnumerable<Type> Get(Dictionary<Assembly, Type[]> dynamicAssemblyTypes)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                if (!assembly.IsDynamic)
                {
                    // GetValue can invoke its callback more than once under a race; Lazy ensures the
                    // assembly is scanned once.
                    types = _typesByAssembly.GetValue(
                        assembly,
                        key => new Lazy<Type[]>(() => findInitializerTypes(key), LazyThreadSafetyMode.ExecutionAndPublication)).Value;
                }
                else if (!dynamicAssemblyTypes.TryGetValue(assembly, out types!))
                {
                    types = findInitializerTypes(assembly);
                    dynamicAssemblyTypes[assembly] = types;
                }

                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }
    }
}
