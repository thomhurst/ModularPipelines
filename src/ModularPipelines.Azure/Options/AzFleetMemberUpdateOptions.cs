using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "member", "update")]
public record AzFleetMemberUpdateOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--update-group")]
    public string? UpdateGroup { get; set; }
}