using ModularPipelines.Distributed;

namespace ModularPipelines;

/// <summary>
/// A planning-safe run condition that is satisfied on machines that provide <see cref="Capability"/>.
/// </summary>
/// <remarks>
/// In distributed mode the master does not evaluate these conditions locally. It adds the capability
/// to the module's <see cref="CapabilityRequirement"/> so that only a matching worker receives the module.
/// <see cref="RunIfAttribute{T}"/>, <see cref="RunIfAllAttribute{T1,T2}"/>, <see cref="RunIfAnyAttribute{T1,T2}"/>,
/// and <see cref="ConditionGroup"/> compositions of these conditions translate into the equivalent requirement.
/// </remarks>
/// <example>
/// <code>
/// public sealed class OnGpuMachine : ICapabilityCondition
/// {
///     public Capability Capability => Capability.Gpu;
///
///     public Task&lt;bool&gt; EvaluateAsync(IPipelineContext context) =&gt; ...;
/// }
/// </code>
/// </example>
public interface ICapabilityCondition : IPlanningRunCondition
{
    /// <summary>
    /// Gets the capability a machine must provide for this condition to be satisfied.
    /// </summary>
    Capability Capability { get; }
}
