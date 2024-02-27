using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipOnMainBranch]
[SkipIfDependabot]
[DependsOn<ChangedFilesInPullRequestModule>]
public class CheckReleaseNotesAddedModule : Module
{
    private const string MissingReleaseNotesMessage = "No release notes for this change. Please add some notes to the ReleaseNotes.md file.";

    /// <inheritdoc/>
    protected override async Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        var getChangedFilesInPullRequestModule = await GetModule<ChangedFilesInPullRequestModule>();

        return getChangedFilesInPullRequestModule.SkipDecision;
    }

    /// <inheritdoc/>
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var getChangedFilesInPullRequestModule = await GetModule<ChangedFilesInPullRequestModule>();

        var releaseNotesFile = context.Git().RootDirectory.GetFolder("src").GetFolder("ModularPipelines.Build").GetFile("ReleaseNotes.md");

        var changedFiles = getChangedFilesInPullRequestModule.Value!;

        if (!releaseNotesFile.Exists
            || string.IsNullOrEmpty(await releaseNotesFile.ReadAsync(cancellationToken))
            || NeedsToUpdateReleaseNotes(changedFiles))
        {
            throw new Exception(MissingReleaseNotesMessage);
        }

        return await NothingAsync();
    }

    private static bool NeedsToUpdateReleaseNotes(IReadOnlyList<File> changedFiles)
    {
        if (changedFiles.Any(x => x.Name.EndsWith("ReleaseNotes.md")))
        {
            return false;
        }

        return changedFiles.Any(x => x.Name.EndsWith(".cs"));
    }
}
