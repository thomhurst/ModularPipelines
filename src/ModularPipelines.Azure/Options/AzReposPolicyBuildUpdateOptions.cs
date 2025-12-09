using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "build", "update")]
public record AzReposPolicyBuildUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--blocking")]
    public bool? Blocking { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CliOption("--build-definition-id")]
    public string? BuildDefinitionId { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliFlag("--manual-queue-only")]
    public bool? ManualQueueOnly { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path-filter")]
    public string? PathFilter { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--queue-on-source-update-only")]
    public bool? QueueOnSourceUpdateOnly { get; set; }

    [CliOption("--repository-id")]
    public string? RepositoryId { get; set; }

    [CliOption("--valid-duration")]
    public string? ValidDuration { get; set; }
}