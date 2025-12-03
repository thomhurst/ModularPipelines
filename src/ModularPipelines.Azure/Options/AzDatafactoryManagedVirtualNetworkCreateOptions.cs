using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "managed-virtual-network", "create")]
public record AzDatafactoryManagedVirtualNetworkCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--managed-virtual-network-name")] string ManagedVirtualNetworkName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}