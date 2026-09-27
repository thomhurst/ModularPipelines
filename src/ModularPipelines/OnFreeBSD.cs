using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Distributed;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running on FreeBSD.
/// </summary>
/// <example>
/// <code>
/// [RunIf&lt;OnFreeBSD&gt;]
/// public class FreeBsdModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnFreeBSD : ICapabilityCondition
{
    /// <inheritdoc />
    public Capability Capability => Capability.FreeBSD;

    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineContext context)
        => Task.FromResult(OperatingSystem.IsFreeBSD());
}
