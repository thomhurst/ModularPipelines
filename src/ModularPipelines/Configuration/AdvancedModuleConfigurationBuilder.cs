using Kevlar;
using ModularPipelines.Context;

namespace ModularPipelines.Configuration;

/// <summary>
/// Provides advanced module configuration using Kevlar resilience shields.
/// </summary>
public sealed class AdvancedModuleConfigurationBuilder
{
    private readonly ModuleConfigurationBuilder _builder;

    internal AdvancedModuleConfigurationBuilder(ModuleConfigurationBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>
    /// Sets a custom Kevlar shield for module execution.
    /// </summary>
    /// <param name="shield">The Kevlar shield to execute around the module.</param>
    /// <returns>The parent module configuration builder.</returns>
    public ModuleConfigurationBuilder WithShield(Shield shield)
    {
        ArgumentNullException.ThrowIfNull(shield);
        return _builder.SetResilienceShield(_ => shield);
    }

    /// <summary>
    /// Sets a factory that creates a custom Kevlar shield from the module context.
    /// </summary>
    /// <param name="factory">The shield factory.</param>
    /// <returns>The parent module configuration builder.</returns>
    public ModuleConfigurationBuilder WithShield(Func<IModuleContext, Shield> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        return _builder.SetResilienceShield(factory);
    }
}
