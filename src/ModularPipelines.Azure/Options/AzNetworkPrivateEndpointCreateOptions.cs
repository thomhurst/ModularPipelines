using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-endpoint", "create")]
public record AzNetworkPrivateEndpointCreateOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--private-connection-resource-id")] string PrivateConnectionResourceId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--asg")]
    public string? Asg { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--ip-config")]
    public string? IpConfig { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--manual-request")]
    public bool? ManualRequest { get; set; }

    [CommandSwitch("--nic-name")]
    public string? NicName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--request-message")]
    public string? RequestMessage { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}