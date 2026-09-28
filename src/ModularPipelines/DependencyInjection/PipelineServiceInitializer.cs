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
/// This keeps the discovery rules and batch ordering of
/// <c>Initialization.Microsoft.Extensions.DependencyInjection</c>'s <c>InitializeAsync</c>,
/// but scans loaded types at most once per pipeline instead of once per factory registration,
/// and caches the initializer types of each non-dynamic assembly for the life of the process.
/// The library's per-factory scan made every pipeline build CPU-bound in proportion to
/// (factory registrations x loaded types).
/// </remarks>
internal static class PipelineServiceInitializer
{
    private static readonly ConditionalWeakTable<Assembly, Type[]> InitializerTypesByAssembly = [];

    public static Task InitializeAsync(IServiceProvider serviceProvider) =>
        InitializeAsync(serviceProvider, GetLoadedInitializerTypes);

    /// <param name="serviceProvider">The pipeline service provider.</param>
    /// <param name="getLoadedInitializerTypes">
    /// Returns every loaded type that implements <see cref="IInitializer"/>. Called at most once,
    /// and only when a factory registration's return type is not itself an initializer.
    /// </param>
    internal static async Task InitializeAsync(
        IServiceProvider serviceProvider,
        Func<IReadOnlyList<Type>> getLoadedInitializerTypes)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(getLoadedInitializerTypes);

        var descriptors = serviceProvider
            .GetRequiredService<IPipelineServiceContainerWrapper>()
            .ServiceCollection
            .Where(descriptor => !descriptor.IsKeyedService
                                 && !descriptor.ServiceType.ContainsGenericParameters)
            .ToArray();

        var loadedInitializerTypes = new Lazy<IReadOnlyList<Type>>(getLoadedInitializerTypes);
        var initializerDescriptors = descriptors
            .Where(descriptor => descriptor.ImplementationInstance is IInitializer)
            .Concat(descriptors.Where(descriptor => descriptor.ImplementationType is { } implementationType
                                                    && IsInitializer(implementationType)))
            .Concat(descriptors.Where(descriptor => descriptor.ImplementationFactory is { } factory
                                                    && CanProduceInitializer(factory.Method.ReturnType, loadedInitializerTypes)))
            .ToArray();

        var initializers = ResolveInitializers(serviceProvider, descriptors, initializerDescriptors);

        foreach (var batch in initializers.GroupBy(initializer => initializer.Order).OrderBy(batch => batch.Key))
        {
            // Start every initializer in the batch before awaiting, so a synchronous throw
            // cannot leave already-started initializers running unobserved.
            await Task.WhenAll([.. batch.Select(StartInitializer)]).ConfigureAwait(false);
        }
    }

    private static List<IInitializer> ResolveInitializers(
        IServiceProvider serviceProvider,
        ServiceDescriptor[] descriptors,
        ServiceDescriptor[] initializerDescriptors)
    {
        // Each descriptor's position among registrations of the same service type, so duplicate
        // service types resolve to their own implementation rather than the last registration.
        var registrationIndexes = new Dictionary<ServiceDescriptor, int>(ReferenceEqualityComparer.Instance);
        var registrationCounts = new Dictionary<Type, int>();
        foreach (var descriptor in descriptors)
        {
            registrationCounts.TryGetValue(descriptor.ServiceType, out var index);
            registrationIndexes[descriptor] = index;
            registrationCounts[descriptor.ServiceType] = index + 1;
        }

        var resolvedServices = new Dictionary<Type, object?[]>();
        var initializers = new List<IInitializer>();
        var seen = new HashSet<IInitializer>(ReferenceEqualityComparer.Instance);
        foreach (var descriptor in initializerDescriptors)
        {
            EnsureSingleton(descriptor);

            var service = registrationCounts[descriptor.ServiceType] == 1
                ? serviceProvider.GetService(descriptor.ServiceType)
                : ResolveRegistration(serviceProvider, descriptor, registrationIndexes[descriptor], resolvedServices);

            // Forwarding registrations commonly expose one singleton through several service types;
            // initialize each instance once.
            if (service is IInitializer initializer && seen.Add(initializer))
            {
                initializers.Add(initializer);
            }
        }

        return initializers;
    }

    private static object? ResolveRegistration(
        IServiceProvider serviceProvider,
        ServiceDescriptor descriptor,
        int registrationIndex,
        Dictionary<Type, object?[]> resolvedServices)
    {
        if (!RuntimeFeature.IsDynamicCodeSupported)
        {
            // Resolving IEnumerable<T> for a runtime type needs dynamic code; Native AOT keeps the
            // library's behavior of resolving the last registration.
            return serviceProvider.GetService(descriptor.ServiceType);
        }

        if (!resolvedServices.TryGetValue(descriptor.ServiceType, out var services))
        {
            services = [.. serviceProvider.GetServices(descriptor.ServiceType)];
            resolvedServices[descriptor.ServiceType] = services;
        }

        return registrationIndex < services.Length
            ? services[registrationIndex]
            : serviceProvider.GetService(descriptor.ServiceType);
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

    private static void EnsureSingleton(ServiceDescriptor descriptor)
    {
        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            return;
        }

        var implementationType = descriptor.ImplementationType
                                 ?? descriptor.ImplementationInstance?.GetType()
                                 ?? descriptor.ServiceType;
        throw new InvalidOperationException(
            $"Service Provider Initializers are only supported for Singletons. {implementationType.Name} is {descriptor.Lifetime}");
    }

    private static bool CanProduceInitializer(Type factoryReturnType, Lazy<IReadOnlyList<Type>> loadedInitializerTypes)
    {
        return IsInitializer(factoryReturnType)
               || loadedInitializerTypes.Value.Any(factoryReturnType.IsAssignableFrom);
    }

    private static bool IsInitializer(Type type) => typeof(IInitializer).IsAssignableFrom(type);

    private static IReadOnlyList<Type> GetLoadedInitializerTypes()
    {
        var initializerTypes = new List<Type>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Dynamic assemblies (for example mocking proxies) can gain types after the first scan.
            initializerTypes.AddRange(assembly.IsDynamic
                ? FindInitializerTypes(assembly)
                : InitializerTypesByAssembly.GetValue(assembly, FindInitializerTypes));
        }

        return initializerTypes;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Matches the Initialization library's loaded-type scan. A factory can only return a type the trimmer kept, so types removed by trimming cannot affect which factories produce initializers.")]
    private static Type[] FindInitializerTypes(Assembly assembly) =>
        [.. AssemblyTypeLoader.GetLoadableTypes(assembly).Where(IsInitializer)];
}
