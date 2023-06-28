using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class NugetVersionGeneratorModule : Module<string>
{
    protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();

        var branchName = context.Git().Information.BranchName;
        var commitDate = context.Git().Information.LastCommitDateTime;

        return $"{GitVersionInformation.Major}.{GitVersionInformation.Minor}.{GitVersionInformation.Patch}-{GitVersionInformation.PreReleaseLabel}-{GitVersionInformation.CommitsSinceVersionSource}";
    }
}