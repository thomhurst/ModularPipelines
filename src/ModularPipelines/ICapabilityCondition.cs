using ModularPipelines.Distributed;

namespace ModularPipelines;

/// <summary>
/// A planning-safe run condition that is satisfied on machines that provide <see cref="Capability"/>.
/// </summary>
/// <remarks>
/// In distributed mode the master does not evaluate these conditions locally. It adds the capability
/// to the module's <see cref="CapabilityRequirement"/> so that only a matching worker receives the module.
/// <see cref="RunIfAttribute{T}"/>, <see cref="RunIfAttribute{T1,T2}"/>, <see cref="RunIfAnyAttribute{T1,T2}"/>,
/// and <see cref="ConditionGroup"/> compositions of these conditions translate into the equivalent requirement.
/// <para>
/// The default <see cref="IRunCondition.EvaluateAsync"/> implementation checks the executing process's
/// declared capabilities, including registered <see cref="ICapabilityProvider"/> results. Implementers
/// only need to provide <see cref="Capability"/>. Put capability detection in a provider so routing and
/// execution use the same declaration.
/// </para>
/// <para>
/// An explicit evaluation implementation still runs during local or worker execution. It is not called
/// by the distributed master when constructing a route; the master uses <see cref="Capability"/> only.
/// Such an implementation can add an execution-time restriction, but must not assume that it controls routing.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public sealed class OnGpuMachine : ICapabilityCondition
/// {
///     public Capability Capability => Capability.Gpu;
/// }
/// </code>
/// </example>
public interface ICapabilityCondition : IRunCondition, IPlanningSafe
{
    /// <summary>
    /// Gets the capability a machine must provide for this condition to be satisfied.
    /// </summary>
    Capability Capability { get; }

    async Task<bool> IRunCondition.EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var registry = context.Services.GetRequiredService<LocalCapabilityRegistry>();
        var capabilities = await registry.GetAsync(cancellationToken).ConfigureAwait(false);
        return capabilities.Contains(Capability);
    }
}
