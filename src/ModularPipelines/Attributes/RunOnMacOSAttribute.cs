using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

[ExcludeFromCodeCoverage]
public class RunOnMacOSAttribute : RunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsMacOS());
    }
}