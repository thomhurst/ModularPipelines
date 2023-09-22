using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class RunOnMacOSAttribute : RunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsMacOS());
    }
}