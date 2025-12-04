using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "kubernetescluster", "update")]
public record AzNetworkcloudKubernetesclusterUpdateOptions : AzOptions
{
    [CliOption("--control-plane-node-configuration")]
    public string? ControlPlaneNodeConfiguration { get; set; }

    [CliFlag("--generate-ssh-keys")]
    public bool? GenerateSshKeys { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kubernetes-cluster-name")]
    public string? KubernetesClusterName { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--ssh-dest-key-path")]
    public string? SshDestKeyPath { get; set; }

    [CliOption("--ssh-key-values")]
    public string? SshKeyValues { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}