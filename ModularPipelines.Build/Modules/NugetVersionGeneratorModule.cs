using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
// ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162

namespace ModularPipelines.Build.Modules;

public class NugetVersionGeneratorModule : Module<string>
{
    protected override async Task<ModuleResult<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitVersionInformation = await context.Git().Versioning.GetGitVersioningInformation();

        if (gitVersionInformation.BranchName == "main")
        {
            return gitVersionInformation.SemVer!;
        }

        return $"{gitVersionInformation.Major}.{gitVersionInformation.Minor}.{gitVersionInformation.Patch}-{gitVersionInformation.PreReleaseLabel}-{gitVersionInformation.CommitsSinceVersionSource}";
    }

    protected override async Task OnAfterExecute(IModuleContext context)
    {
        var moduleResult = await this;
        context.Logger.LogInformation("NuGet Version to Package: {Version}", moduleResult.Value);
    }
}
