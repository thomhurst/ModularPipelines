using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipOnMainBranch]
public class ChangedFilesInPullRequestModule : Module<IReadOnlyList<File>>
{
    protected override async Task<IReadOnlyList<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var result = await context.Git().Commands.Diff(new GitDiffOptions
        {
            NameOnly = true,
            Arguments = ["origin/main"],
        }, cancellationToken);

        return result.StandardOutput.Split(Environment.NewLine,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new File(Path.Combine(context.Git().RootDirectory, x)))
            .ToList();
    }
}