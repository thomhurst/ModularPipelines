using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Build.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipOnMainBranch : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipOnMainBranch);

    public Task<bool> EvaluateAsync(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName == "main");
    }
}
