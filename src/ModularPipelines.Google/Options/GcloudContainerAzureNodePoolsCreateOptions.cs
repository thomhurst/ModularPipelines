using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "node-pools", "create")]
public record GcloudContainerAzureNodePoolsCreateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--max-pods-per-node")] string MaxPodsPerNode,
[property: CommandSwitch("--node-version")] string NodeVersion,
[property: CommandSwitch("--ssh-public-key")] string SshPublicKey,
[property: CommandSwitch("--subnet-id")] string SubnetId,
[property: CommandSwitch("--max-nodes")] string MaxNodes,
[property: CommandSwitch("--min-nodes")] string MinNodes
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--azure-availability-zone")]
    public string? AzureAvailabilityZone { get; set; }

    [CommandSwitch("--config-encryption-key-id")]
    public string? ConfigEncryptionKeyId { get; set; }

    [CommandSwitch("--config-encryption-public-key")]
    public string? ConfigEncryptionPublicKey { get; set; }

    [BooleanCommandSwitch("--enable-autorepair")]
    public bool? EnableAutorepair { get; set; }

    [CommandSwitch("--node-labels")]
    public string[]? NodeLabels { get; set; }

    [CommandSwitch("--node-taints")]
    public string[]? NodeTaints { get; set; }

    [CommandSwitch("--root-volume-size")]
    public string? RootVolumeSize { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }

    [CommandSwitch("--proxy-resource-group-id")]
    public string? ProxyResourceGroupId { get; set; }

    [CommandSwitch("--proxy-secret-id")]
    public string? ProxySecretId { get; set; }
}