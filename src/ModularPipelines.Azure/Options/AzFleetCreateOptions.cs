using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "create")]
public record AzFleetCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--agent-subnet-id")]
    public string? AgentSubnetId { get; set; }

    [CliOption("--apiserver-subnet-id")]
    public string? ApiserverSubnetId { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--dns-name-prefix")]
    public string? DnsNamePrefix { get; set; }

    [CliFlag("--enable-hub")]
    public bool? EnableHub { get; set; }

    [CliFlag("--enable-managed-identity")]
    public bool? EnableManagedIdentity { get; set; }

    [CliFlag("--enable-private-cluster")]
    public bool? EnablePrivateCluster { get; set; }

    [CliFlag("--enable-vnet-integration")]
    public bool? EnableVnetIntegration { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }
}