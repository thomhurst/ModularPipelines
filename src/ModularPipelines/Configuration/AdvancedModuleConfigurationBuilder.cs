using ModularPipelines.Context;
using Polly;

namespace ModularPipelines.Configuration;

/// <summary>
/// Provides advanced module configuration that depends on third-party policy abstractions.
/// </summary>
public sealed class AdvancedModuleConfigurationBuilder
{
    private readonly ModuleConfigurationBuilder _builder;

    internal AdvancedModuleConfigurationBuilder(ModuleConfigurationBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>
    /// Sets a custom Polly async policy for module execution.
    /// </summary>
    /// <param name="policy">The Polly async policy to execute around the module.</param>
    /// <returns>The parent module configuration builder.</returns>
    public ModuleConfigurationBuilder WithRetryPolicy(IAsyncPolicy policy)
    {
        ArgumentNullException.ThrowIfNull(policy);
        return _builder.SetAdvancedRetryPolicy(_ => policy);
    }

    /// <summary>
    /// Sets a factory that creates a custom Polly async policy from the module context.
    /// </summary>
    /// <param name="factory">The policy factory.</param>
    /// <returns>The parent module configuration builder.</returns>
    public ModuleConfigurationBuilder WithRetryPolicy(Func<IModuleContext, IAsyncPolicy> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        return _builder.SetAdvancedRetryPolicy(factory);
    }
}
