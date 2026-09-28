using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Distributed;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running on Windows.
/// </summary>
/// <example>
/// <code>
/// [RunIf&lt;OnWindows&gt;]
/// public class WindowsOnlyModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnWindows : ICapabilityCondition
{
    /// <inheritdoc />
    public Capability Capability => Capability.Windows;

    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
        => Task.FromResult(OperatingSystem.IsWindows());
}
