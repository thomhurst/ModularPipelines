using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "post-commit")]
public record AzNetworkManagerPostCommitOptions(
[property: CommandSwitch("--commit-type")] string CommitType,
[property: CommandSwitch("--target-locations")] string TargetLocations
) : AzOptions
{
    [CommandSwitch("--configuration-ids")]
    public string? ConfigurationIds { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}