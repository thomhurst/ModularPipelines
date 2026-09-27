using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ModularPipelines.Distributed;

/// <summary>
/// Extension methods for declaring the capabilities this process provides.
/// </summary>
/// <remarks>
/// Capabilities apply in every mode. Locally, modules whose <c>[RequiresCapability]</c> or
/// <c>[RequiresAnyCapability]</c> requirements are not satisfied are skipped. In distributed mode,
/// workers advertise these capabilities so the master can route modules to them.
/// </remarks>
public static class CapabilityPipelineBuilderExtensions
{
    /// <summary>
    /// Declares capabilities this process provides, in addition to the detected operating system.
    /// </summary>
    /// <param name="builder">The pipeline builder to configure.</param>
    /// <param name="capabilities">The capabilities to advertise.</param>
    /// <returns>The same pipeline builder, for chaining.</returns>
    public static PipelineBuilder AddCapabilities(this PipelineBuilder builder, params Capability[] capabilities)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(capabilities);
        if (capabilities.Any(static capability => string.IsNullOrWhiteSpace(capability.Name)))
        {
            throw new ArgumentException("A capability name cannot be empty.", nameof(capabilities));
        }

        builder.Services.AddSingleton<ICapabilityProvider>(new StaticCapabilityProvider([.. capabilities]));
        return builder;
    }

    /// <summary>
    /// Registers a provider that detects capabilities this process provides.
    /// </summary>
    /// <typeparam name="TProvider">The provider type.</typeparam>
    /// <param name="builder">The pipeline builder to configure.</param>
    /// <returns>The same pipeline builder, for chaining.</returns>
    public static PipelineBuilder AddCapabilityProvider<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TProvider>(
        this PipelineBuilder builder)
        where TProvider : class, ICapabilityProvider
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ICapabilityProvider, TProvider>());
        return builder;
    }

    private sealed class StaticCapabilityProvider(Capability[] capabilities) : ICapabilityProvider
    {
        public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IEnumerable<Capability>>(capabilities);
    }
}
