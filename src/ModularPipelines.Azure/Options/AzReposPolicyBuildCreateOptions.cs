using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "build", "create")]
public record AzReposPolicyBuildCreateOptions(
[property: BooleanCommandSwitch("--blocking")] bool Blocking,
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--build-definition-id")] string BuildDefinitionId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: BooleanCommandSwitch("--manual-queue-only")] bool ManualQueueOnly,
[property: BooleanCommandSwitch("--queue-on-source-update-only")] bool QueueOnSourceUpdateOnly,
[property: CommandSwitch("--repository-id")] string RepositoryId,
[property: CommandSwitch("--valid-duration")] string ValidDuration
) : AzOptions
{
    [CommandSwitch("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--path-filter")]
    public string? PathFilter { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}