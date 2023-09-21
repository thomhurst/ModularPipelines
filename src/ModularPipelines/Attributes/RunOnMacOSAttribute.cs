using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class RunOnMacOSAttribute : RunConditionAttribute
{
    public override bool Condition(IPipelineContext pipelineContext)
    {
        return OperatingSystem.IsMacOS();
    }
}