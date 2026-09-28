using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipOnMainBranch : RunConditionAttribute
{
    public SkipOnMainBranch()
        : base(ConditionIntent.Skip)
    {
    }

    public override string ConditionNames => nameof(SkipOnMainBranch);

    public override async Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        var repositoryInfo = await pipelineContext.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false);
        return repositoryInfo?.BranchName == "main";
    }
}
