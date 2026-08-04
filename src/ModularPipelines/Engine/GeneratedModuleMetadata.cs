using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Extensions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores module discovery, registration, and dependency metadata emitted by
/// <c>ModularPipelines.SourceGenerator</c>.
/// </summary>
public static class GeneratedModuleMetadata
{
    private static readonly object RegistrationLock = new();
    private static readonly ConcurrentDictionary<Assembly, AssemblyModuleMetadata> Assemblies = new();
    private static readonly ConcurrentDictionary<Type, GeneratedModuleRegistration> Modules = new();

    /// <summary>
    /// Creates trim-safe registration metadata for one module.
    /// </summary>
    /// <returns>The generated registration metadata.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static GeneratedModuleRegistration CreateRegistration<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(
        IReadOnlyList<ModuleDependencyMetadata> dependencies,
        bool dependenciesComplete = true)
        where TModule : class, IModule
    {
        return new GeneratedModuleRegistration(
            typeof(TModule),
            static services => services.AddModule<TModule>(),
            dependencies.ToArray(),
            dependenciesComplete);
    }

    /// <summary>
    /// Creates trim-safe registration and runtime metadata for one typed module.
    /// </summary>
    /// <returns>The generated registration metadata.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static GeneratedModuleRegistration CreateRegistration<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule,
        TResult>(
        IReadOnlyList<ModuleDependencyMetadata> dependencies,
        bool dependenciesComplete = true)
        where TModule : Module<TResult>
    {
        return new GeneratedModuleRegistration(
            typeof(TModule),
            static services => services.AddModule<TModule>(),
            dependencies.ToArray(),
            dependenciesComplete)
        {
            Runtime = new GeneratedModuleRuntime<TModule, TResult>(),
        };
    }

    /// <summary>
    /// Registers generated module metadata for one assembly.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Register(
        Assembly assembly,
        IReadOnlyList<GeneratedModuleRegistration> registrations,
        bool isComplete = true)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentNullException.ThrowIfNull(registrations);

        var registrationsByType = registrations.ToDictionary(static registration => registration.ModuleType);
        if (registrationsByType.Keys.Any(moduleType => moduleType.Assembly != assembly))
        {
            throw new ArgumentException(
                "Every generated module must belong to the registered assembly.",
                nameof(registrations));
        }

        lock (RegistrationLock)
        {
            if (Assemblies.ContainsKey(assembly))
            {
                throw new InvalidOperationException(
                    $"Module metadata is already registered for {assembly.FullName}.");
            }

            var duplicateModuleType = registrationsByType.Keys.FirstOrDefault(Modules.ContainsKey);
            if (duplicateModuleType is not null)
            {
                throw new InvalidOperationException(
                    $"Module metadata is already registered for {duplicateModuleType}.");
            }

            var normalizedRegistrations = registrationsByType.Values
                .Select(static registration => registration with
                {
                    Dependencies = registration.Dependencies.ToArray(),
                })
                .ToArray();
            Assemblies[assembly] = new AssemblyModuleMetadata(normalizedRegistrations, isComplete);
            foreach (var registration in normalizedRegistrations)
            {
                Modules[registration.ModuleType] = registration;
            }
        }
    }

    internal static bool TryGetModuleTypes(
        Assembly assembly,
        out IReadOnlyList<Type> moduleTypes,
        out bool isComplete)
    {
        if (Assemblies.TryGetValue(assembly, out var metadata))
        {
            moduleTypes = metadata.Registrations
                .Select(static registration => registration.ModuleType)
                .ToArray();
            isComplete = metadata.IsComplete;
            return true;
        }

        moduleTypes = Array.Empty<Type>();
        isComplete = false;
        return false;
    }

    internal static bool TryGetDependencies(
        Type moduleType,
        out IReadOnlyList<ModuleDependencyMetadata> dependencies)
    {
        if (Modules.TryGetValue(moduleType, out var registration)
            && registration.DependenciesComplete)
        {
            dependencies = registration.Dependencies;
            return true;
        }

        dependencies = Array.Empty<ModuleDependencyMetadata>();
        return false;
    }

    internal static bool TryRegisterModule(IServiceCollection services, Type moduleType)
    {
        if (!Modules.TryGetValue(moduleType, out var registration))
        {
            return false;
        }

        registration.Register(services);
        return true;
    }

    internal static bool TryGetRuntime(Type moduleType, out IGeneratedModuleRuntime runtime)
    {
        if (Modules.TryGetValue(moduleType, out var registration)
            && registration.Runtime is not null)
        {
            runtime = registration.Runtime;
            return true;
        }

        runtime = null!;
        return false;
    }

    private sealed record AssemblyModuleMetadata(
        IReadOnlyList<GeneratedModuleRegistration> Registrations,
        bool IsComplete);
}

/// <summary>
/// Describes one generated module and provides its reflection-free DI registration.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed record GeneratedModuleRegistration(
    Type ModuleType,
    Action<IServiceCollection> Register,
    IReadOnlyList<ModuleDependencyMetadata> Dependencies,
    bool DependenciesComplete = true)
{
    internal IGeneratedModuleRuntime? Runtime { get; init; }
}

/// <summary>
/// Describes a statically declared module dependency.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed record ModuleDependencyMetadata(Type DependencyType, bool Optional);

internal interface IGeneratedModuleRuntime
{
    ModuleExecutionContext CreateExecutionContext(IModule module, Type moduleType);

    IModuleResult CreateFailure(
        Exception exception,
        ModuleExecutionContext executionContext);

    IModuleResult CreateSkipped(ModuleExecutionContext executionContext);

    IModuleLogger GetLogger(IServiceProvider serviceProvider);

    ILogger GetOutputLogger(IServiceProvider serviceProvider);

    void CancelCompletionSource(IModule module);

    void SetCompletionSource(IModule module, IModuleResult result);

    Task<IModuleResult> ExecuteAsync(
        IModuleExecutionPipeline pipeline,
        IModule module,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        Func<CancellationToken, Task>? prepareExecutionAsync,
        Func<IModuleResult, CancellationToken, Task>? finalizeExecutionAsync,
        bool completeModule,
        CancellationToken cancellationToken);
}

internal sealed class GeneratedModuleRuntime<TModule, TResult> : IGeneratedModuleRuntime
    where TModule : Module<TResult>
{
    public ModuleExecutionContext CreateExecutionContext(IModule module, Type moduleType)
    {
        return new ModuleExecutionContext<TResult>((Module<TResult>) module, moduleType);
    }

    public IModuleResult CreateFailure(
        Exception exception,
        ModuleExecutionContext executionContext)
    {
        return ModuleResult<TResult>.CreateFailure(
            exception,
            (ModuleExecutionContext<TResult>) executionContext);
    }

    public IModuleResult CreateSkipped(ModuleExecutionContext executionContext)
    {
        return ModuleResult<TResult>.CreateSkipped(
            executionContext.SkipResult ?? SkipDecision.DoNotSkip,
            (ModuleExecutionContext<TResult>) executionContext);
    }

    public IModuleLogger GetLogger(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<ModuleLogger<TModule>>();
    }

    public ILogger GetOutputLogger(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<ILogger<TModule>>();
    }

    public void CancelCompletionSource(IModule module)
    {
        ((Module<TResult>) module).CompletionSource.TrySetCanceled();
    }

    public void SetCompletionSource(IModule module, IModuleResult result)
    {
        ((Module<TResult>) module).CompletionSource.TrySetResult((ModuleResult<TResult>) result);
    }

    public async Task<IModuleResult> ExecuteAsync(
        IModuleExecutionPipeline pipeline,
        IModule module,
        ModuleExecutionContext executionContext,
        IModuleContext moduleContext,
        Func<CancellationToken, Task>? prepareExecutionAsync,
        Func<IModuleResult, CancellationToken, Task>? finalizeExecutionAsync,
        bool completeModule,
        CancellationToken cancellationToken)
    {
        return await pipeline.ExecuteAsync(
                (Module<TResult>) module,
                (ModuleExecutionContext<TResult>) executionContext,
                moduleContext,
                cancellationToken,
                prepareExecutionAsync,
                finalizeExecutionAsync,
                completeModule)
            .ConfigureAwait(false);
    }
}
