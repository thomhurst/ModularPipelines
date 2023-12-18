using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "create")]
public record AzFleetCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--agent-subnet-id")]
    public string? AgentSubnetId { get; set; }

    [CommandSwitch("--apiserver-subnet-id")]
    public string? ApiserverSubnetId { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--dns-name-prefix")]
    public string? DnsNamePrefix { get; set; }

    [BooleanCommandSwitch("--enable-hub")]
    public bool? EnableHub { get; set; }

    [BooleanCommandSwitch("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [BooleanCommandSwitch("--enable-private-cluster")]
    public bool? EnablePrivateCluster { get; set; }

    [BooleanCommandSwitch("--enable-vnet-integration")]
    public bool? EnableVnetIntegration { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }
}