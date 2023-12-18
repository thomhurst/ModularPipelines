using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "build", "update")]
public record AzReposPolicyBuildUpdateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--blocking")]
    public bool? Blocking { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--branch-match-type")]
    public string? BranchMatchType { get; set; }

    [CommandSwitch("--build-definition-id")]
    public string? BuildDefinitionId { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [BooleanCommandSwitch("--manual-queue-only")]
    public bool? ManualQueueOnly { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--path-filter")]
    public string? PathFilter { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--queue-on-source-update-only")]
    public bool? QueueOnSourceUpdateOnly { get; set; }

    [CommandSwitch("--repository-id")]
    public string? RepositoryId { get; set; }

    [CommandSwitch("--valid-duration")]
    public string? ValidDuration { get; set; }
}

