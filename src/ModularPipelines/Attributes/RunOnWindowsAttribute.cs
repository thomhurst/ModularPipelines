using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

[ExcludeFromCodeCoverage]
public class RunOnWindowsAttribute : RunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsWindows());
    }
}