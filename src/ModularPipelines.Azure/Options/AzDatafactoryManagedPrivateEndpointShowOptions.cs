using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "managed-private-endpoint", "show")]
public record AzDatafactoryManagedPrivateEndpointShowOptions : AzOptions
{
    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--managed-private-endpoint-name")]
    public string? ManagedPrivateEndpointName { get; set; }

    [CliOption("--managed-virtual-network-name")]
    public string? ManagedVirtualNetworkName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}