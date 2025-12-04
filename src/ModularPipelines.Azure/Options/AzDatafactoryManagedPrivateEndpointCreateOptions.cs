using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "managed-private-endpoint", "create")]
public record AzDatafactoryManagedPrivateEndpointCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--managed-private-endpoint-name")] string ManagedPrivateEndpointName,
[property: CliOption("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--fqdns")]
    public string? Fqdns { get; set; }

    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--private-link")]
    public string? PrivateLink { get; set; }
}