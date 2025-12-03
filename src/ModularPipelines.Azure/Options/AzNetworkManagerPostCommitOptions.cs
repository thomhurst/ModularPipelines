using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "post-commit")]
public record AzNetworkManagerPostCommitOptions(
[property: CliOption("--commit-type")] string CommitType,
[property: CliOption("--target-locations")] string TargetLocations
) : AzOptions
{
    [CliOption("--configuration-ids")]
    public string? ConfigurationIds { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}