using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Abstract base class for attributes that define conditional execution of modules.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class RunConditionAttribute : Attribute
{
    /// <summary>
    /// Evaluates the condition to determine whether the module should run.
    /// </summary>
    /// <param name="pipelineContext">The pipeline context for evaluation.</param>
    /// <returns>A task that represents the asynchronous condition evaluation. The value is true if the module should run; otherwise, false.</returns>
    public abstract Task<bool> Condition(IPipelineHookContext pipelineContext);
}
