using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Default implementation of <see cref="IModuleActivator"/> that sets AsyncLocal context
/// before module construction.
/// </summary>
/// <remarks>
/// This activator ensures that any logging performed during module construction
/// (in constructors or field initializers) will have the correct module context set.
/// The AsyncLocal context is set before construction and restored afterward to support
/// nested scenarios (though module construction is typically not nested).
/// </remarks>
internal sealed class ModuleActivator : IModuleActivator
{
    /// <inheritdoc />
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "This runtime-Type overload is the reflection fallback; generated registrations use the annotated generic overload.")]
    public IModule CreateModule(Type moduleType, IServiceProvider serviceProvider)
    {
        return CreateModuleWithContext(
            moduleType,
            serviceProvider,
            provider => (IModule) ActivatorUtilities.CreateInstance(provider, moduleType),
            initializeConfiguration: true);
    }

    /// <inheritdoc />
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "This runtime-Type overload is the reflection fallback for dependency-graph planning.")]
    public IModule CreatePlanningModule(Type moduleType, IServiceProvider serviceProvider)
    {
        return CreateModuleWithContext(
            moduleType,
            serviceProvider,
            provider => (IModule) ActivatorUtilities.CreateInstance(provider, moduleType),
            initializeConfiguration: false);
    }

    internal static TModule CreateModule<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TModule>(
        IServiceProvider serviceProvider)
        where TModule : class, IModule
    {
        return CreateModuleWithContext(
            typeof(TModule),
            serviceProvider,
            static provider => ActivatorUtilities.CreateInstance<TModule>(provider),
            initializeConfiguration: true);
    }

    private static TModule CreateModuleWithContext<TModule>(
        Type moduleType,
        IServiceProvider serviceProvider,
        Func<IServiceProvider, TModule> activate,
        bool initializeConfiguration)
        where TModule : IModule
    {
        var previousType = ModuleLogger.CurrentModuleType.Value;
        ModuleLogger.CurrentModuleType.Value = moduleType;

        try
        {
            var module = activate(serviceProvider);
            if (initializeConfiguration)
            {
                _ = module.Configuration;
            }

            return module;
        }
        finally
        {
            ModuleLogger.CurrentModuleType.Value = previousType;
        }
    }
}
