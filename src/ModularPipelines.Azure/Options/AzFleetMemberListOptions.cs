using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fleet", "member", "list")]
public record AzFleetMemberListOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;