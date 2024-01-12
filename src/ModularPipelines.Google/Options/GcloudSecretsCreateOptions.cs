using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "create")]
public record GcloudSecretsCreateOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions
{
    [CommandSwitch("--data-file")]
    public string? DataFile { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--set-annotations")]
    public IEnumerable<KeyValue>? SetAnnotations { get; set; }

    [CommandSwitch("--topics")]
    public string[]? Topics { get; set; }

    [CommandSwitch("--expire-time")]
    public string? ExpireTime { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [CommandSwitch("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CommandSwitch("--rotation-period")]
    public string? RotationPeriod { get; set; }

    [CommandSwitch("--replication-policy-file")]
    public string? ReplicationPolicyFile { get; set; }

    [CommandSwitch("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CommandSwitch("--locations")]
    public string[]? Locations { get; set; }

    [CommandSwitch("--replication-policy")]
    public string? ReplicationPolicy { get; set; }
}