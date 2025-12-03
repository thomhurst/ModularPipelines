using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "managed-private-endpoint", "update")]
public record AzDatafactoryManagedPrivateEndpointUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fqdns")]
    public string? Fqdns { get; set; }

    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--managed-private-endpoint-name")]
    public string? ManagedPrivateEndpointName { get; set; }

    [CliOption("--managed-virtual-network-name")]
    public string? ManagedVirtualNetworkName { get; set; }

    [CliOption("--private-link")]
    public string? PrivateLink { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}