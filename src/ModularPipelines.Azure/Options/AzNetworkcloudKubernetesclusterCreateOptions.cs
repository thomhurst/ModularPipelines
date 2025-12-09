using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "kubernetescluster", "create")]
public record AzNetworkcloudKubernetesclusterCreateOptions(
[property: CliOption("--control-plane-node-configuration")] string ControlPlaneNodeConfiguration,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--initial-agent-pool-configurations")] string InitialAgentPoolConfigurations,
[property: CliOption("--kubernetes-cluster-name")] string KubernetesClusterName,
[property: CliOption("--kubernetes-version")] string KubernetesVersion,
[property: CliOption("--network-configuration")] string NetworkConfiguration,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-configuration")]
    public string? AadConfiguration { get; set; }

    [CliOption("--admin-username")]
    public string? AdminUsername { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}