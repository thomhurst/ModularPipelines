using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "member", "create")]
public record AzFleetMemberCreateOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--member-cluster-id")] string MemberClusterId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--update-group")]
    public string? UpdateGroup { get; set; }
}