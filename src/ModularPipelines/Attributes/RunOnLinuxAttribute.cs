using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

[ExcludeFromCodeCoverage]
public class RunOnLinuxAttribute : RunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsLinux());
    }
}