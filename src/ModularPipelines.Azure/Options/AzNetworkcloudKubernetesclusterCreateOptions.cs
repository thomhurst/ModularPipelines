using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "kubernetescluster", "create")]
public record AzNetworkcloudKubernetesclusterCreateOptions(
[property: CommandSwitch("--control-plane-node-configuration")] string ControlPlaneNodeConfiguration,
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--initial-agent-pool-configurations")] string InitialAgentPoolConfigurations,
[property: CommandSwitch("--kubernetes-cluster-name")] string KubernetesClusterName,
[property: CommandSwitch("--kubernetes-version")] string KubernetesVersion,
[property: CommandSwitch("--network-configuration")] string NetworkConfiguration,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aad-configuration")]
    public string? AadConfiguration { get; set; }

    [CommandSwitch("--admin-username")]
    public string? AdminUsername { get; set; }

    [BooleanCommandSwitch("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-resource-group-configuration")]
    public string? ManagedResourceGroupConfiguration { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CommandSwitch("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

