using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class RunOnWindowsAttribute : RunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsWindows());
    }
}