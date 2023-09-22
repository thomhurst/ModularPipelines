using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class RunOnWindowsAttribute : RunConditionAttribute
{
    public override bool Condition(IPipelineContext pipelineContext)
    {
        return OperatingSystem.IsWindows();
    }
}