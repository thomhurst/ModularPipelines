using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

[ExcludeFromCodeCoverage]
public class RunOnWindowsAttribute : RunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsWindows());
    }
}