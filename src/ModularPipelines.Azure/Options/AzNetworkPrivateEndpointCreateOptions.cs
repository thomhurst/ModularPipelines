using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-endpoint", "create")]
public record AzNetworkPrivateEndpointCreateOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--private-connection-resource-id")] string PrivateConnectionResourceId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--asg")]
    public string? Asg { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--ip-config")]
    public string? IpConfig { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--manual-request")]
    public bool? ManualRequest { get; set; }

    [CliOption("--nic-name")]
    public string? NicName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--request-message")]
    public string? RequestMessage { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}