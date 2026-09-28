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
        initializers.AddRange(descriptors
            .Where(descriptor => descriptor.ImplementationFactory is { } factory
                                 && CanProduceInitializer(factory.Method.ReturnType, loadedInitializerTypes))
            .Select(descriptor => GetService(serviceProvider, descriptor))
            .OfType<IInitializer>());

        foreach (var batch in initializers.GroupBy(initializer => initializer.Order).OrderBy(batch => batch.Key))
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

    private static bool CanProduceInitializer(Type factoryReturnType, LoadedInitializerTypes loadedInitializerTypes)
    {
        return IsInitializer(factoryReturnType)
               || loadedInitializerTypes.Get().Any(factoryReturnType.IsAssignableFrom);
    }

    private static bool IsInitializer(Type type) => typeof(IInitializer).IsAssignableFrom(type);

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Matches the Initialization library's loaded-type scan. A factory can only return a type the trimmer kept, so types removed by trimming cannot affect which factories produce initializers.")]
    private static Type[] FindInitializerTypes(Assembly assembly) =>
        [.. AssemblyTypeLoader.GetLoadableTypes(assembly).Where(IsInitializer)];

    /// <summary>
    /// Lists the loaded <see cref="IInitializer"/> types, reading each non-dynamic assembly's types once.
    /// </summary>
    internal sealed class LoadedInitializerTypes(Func<Assembly, Type[]> findInitializerTypes)
    {
        private readonly ConditionalWeakTable<Assembly, Type[]> _typesByAssembly = [];

        public IEnumerable<Type> Get()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Dynamic assemblies (for example mocking proxies) can gain types after a scan.
                var types = assembly.IsDynamic
                    ? findInitializerTypes(assembly)
                    : _typesByAssembly.GetValue(assembly, key => findInitializerTypes(key));
                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }
    }
}
