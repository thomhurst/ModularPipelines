using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class RunOnLinuxAttribute : RunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsLinux());
    }
}