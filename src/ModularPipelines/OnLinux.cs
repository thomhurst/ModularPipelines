using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Distributed;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running on Linux.
/// </summary>
/// <example>
/// <code>
/// [RunIfAny&lt;OnLinux, OnMacOS&gt;]
/// public class UnixModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnLinux : ICapabilityCondition
{
    /// <inheritdoc />
    public Capability Capability => Capability.Linux;

    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineContext context)
        => Task.FromResult(OperatingSystem.IsLinux());
}
