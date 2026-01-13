using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

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
public sealed class OnLinux : IRunCondition
{
    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineHookContext context)
        => Task.FromResult(OperatingSystem.IsLinux());
}
