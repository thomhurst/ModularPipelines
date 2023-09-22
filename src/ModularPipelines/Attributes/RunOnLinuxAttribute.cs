using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class RunOnLinuxAttribute : RunConditionAttribute
{
    public override bool Condition(IPipelineContext pipelineContext)
    {
        return OperatingSystem.IsLinux();
    }
}