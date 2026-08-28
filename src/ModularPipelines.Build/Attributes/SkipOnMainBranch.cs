using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipOnMainBranch : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipOnMainBranch);

    public async Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        var repositoryInfo = await pipelineContext.Tools.Git.Information.GetInfoAsync().ConfigureAwait(false);
        return repositoryInfo?.BranchName == "main";
    }
}
