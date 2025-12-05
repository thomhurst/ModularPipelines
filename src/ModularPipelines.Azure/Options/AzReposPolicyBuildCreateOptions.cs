using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "build", "create")]
public record AzReposPolicyBuildCreateOptions(
[property: CliFlag("--blocking")] bool Blocking,
[property: CliOption("--branch")] string Branch,
[property: CliOption("--build-definition-id")] string BuildDefinitionId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliFlag("--manual-queue-only")] bool ManualQueueOnly,
[property: CliFlag("--queue-on-source-update-only")] bool QueueOnSourceUpdateOnly,
[property: CliOption("--repository-id")] string RepositoryId,
[property: CliOption("--valid-duration")] string ValidDuration
) : AzOptions
{
    [CliOption("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path-filter")]
    public string? PathFilter { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}