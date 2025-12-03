using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "group", "create")]
public record AzNetworkManagerGroupCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--network-manager-name")] string NetworkManagerName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}