using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

/// <summary>
/// A condition that returns true when running on Windows.
/// </summary>
/// <example>
/// <code>
/// [RunIfAll&lt;OnWindows&gt;]
/// public class WindowsOnlyModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnWindows : IRunCondition
{
    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineHookContext context)
        => Task.FromResult(OperatingSystem.IsWindows());
}
