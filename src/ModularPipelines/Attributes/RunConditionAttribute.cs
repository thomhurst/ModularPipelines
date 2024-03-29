using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class RunConditionAttribute : Attribute
{
    public abstract Task<bool> Condition(IPipelineHookContext pipelineContext);
}