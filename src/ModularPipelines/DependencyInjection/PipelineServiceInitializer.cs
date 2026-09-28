using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Runs the <see cref="IInitializer"/> services registered in a pipeline container.
/// </summary>
/// <remarks>
/// This preserves the discovery rules and batch ordering of
/// <c>Initialization.Microsoft.Extensions.DependencyInjection</c>'s <c>InitializeAsync</c>,
/// but scans loaded types once per pipeline instead of once per factory registration.
/// The library's per-factory scan made every pipeline build CPU-bound in proportion to
/// (factory registrations x loaded types).
/// </remarks>
internal static class PipelineServiceInitializer
{
    private static readonly Assembly InitializerAssembly = typeof(IInitializer).Assembly;
    private static readonly string InitializerAssemblyName = InitializerAssembly.GetName().Name!;

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);

        var descriptors = serviceProvider
            .GetRequiredService<IPipelineServiceContainerWrapper>()
            .ServiceCollection
            .Where(descriptor => !descriptor.IsKeyedService)
            .ToArray();

        var initializers = new List<IInitializer>();
        AddInitializers(
            initializers,
            serviceProvider,
            descriptors.Where(descriptor => descriptor.ImplementationInstance is IInitializer));
        AddInitializers(
            initializers,
            serviceProvider,
            descriptors.Where(descriptor => descriptor.ImplementationType is { } implementationType
                && IsInitializer(implementationType)));

        var factoryDescriptors = descriptors
            .Where(descriptor => descriptor.ImplementationFactory is not null)
            .ToArray();
        if (factoryDescriptors.Length > 0)
        {
            var loadedInitializerTypes = new Lazy<Type[]>(GetLoadedInitializerTypes);
            AddInitializers(
                initializers,
                serviceProvider,
                factoryDescriptors.Where(descriptor =>
                    CanProduceInitializer(descriptor.ImplementationFactory!.Method.ReturnType, loadedInitializerTypes)));
        }

        foreach (var batch in initializers.GroupBy(initializer => initializer.Order).OrderBy(batch => batch.Key))
        {
            await Task.WhenAll(batch.Select(initializer => initializer.InitializeAsync())).ConfigureAwait(false);
        }
    }

    private static void AddInitializers(
        List<IInitializer> initializers,
        IServiceProvider serviceProvider,
        IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            if (GetService(serviceProvider, descriptor) is IInitializer initializer)
            {
                initializers.Add(initializer);
            }
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

    private static bool CanProduceInitializer(Type factoryReturnType, Lazy<Type[]> loadedInitializerTypes)
    {
        return IsInitializer(factoryReturnType)
               || loadedInitializerTypes.Value.Any(factoryReturnType.IsAssignableFrom);
    }

    private static bool IsInitializer(Type type) => typeof(IInitializer).IsAssignableFrom(type);

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Factory initializer discovery only inspects types that remain loaded; trimmed types cannot be produced by factories.")]
    private static Type[] GetLoadedInitializerTypes()
    {
        return
        [
            .. AppDomain.CurrentDomain.GetAssemblies()
                .Where(CanDeclareInitializers)
                .SelectMany(AssemblyTypeLoader.GetLoadableTypes)
                .Where(IsInitializer),
        ];
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Assembly references are only used to skip assemblies that cannot implement IInitializer.")]
    private static bool CanDeclareInitializers(Assembly assembly)
    {
        // An IInitializer implementation must reference the assembly that declares the interface.
        // Dynamic assemblies (for example mocking proxies) do not expose reliable references, so keep them.
        return assembly == InitializerAssembly
               || assembly.IsDynamic
               || assembly.GetReferencedAssemblies().Any(reference =>
                   string.Equals(reference.Name, InitializerAssemblyName, StringComparison.Ordinal));
    }
}
