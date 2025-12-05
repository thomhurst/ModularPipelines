using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "node-pools", "create")]
public record GcloudContainerAzureNodePoolsCreateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--max-pods-per-node")] string MaxPodsPerNode,
[property: CliOption("--node-version")] string NodeVersion,
[property: CliOption("--ssh-public-key")] string SshPublicKey,
[property: CliOption("--subnet-id")] string SubnetId,
[property: CliOption("--max-nodes")] string MaxNodes,
[property: CliOption("--min-nodes")] string MinNodes
) : GcloudOptions
{
    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--azure-availability-zone")]
    public string? AzureAvailabilityZone { get; set; }

    [CliOption("--config-encryption-key-id")]
    public string? ConfigEncryptionKeyId { get; set; }

    [CliOption("--config-encryption-public-key")]
    public string? ConfigEncryptionPublicKey { get; set; }

    [CliFlag("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CliOption("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CliOption("--node-taints")]
    public string[]? NodeTaints { get; set; }

    [CliOption("--root-volume-size")]
    public string? RootVolumeSize { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }

    [CliOption("--proxy-resource-group-id")]
    public string? ProxyResourceGroupId { get; set; }

    [CliOption("--proxy-secret-id")]
    public string? ProxySecretId { get; set; }
}