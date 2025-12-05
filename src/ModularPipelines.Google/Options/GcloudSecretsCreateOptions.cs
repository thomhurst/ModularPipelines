using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "create")]
public record GcloudSecretsCreateOptions(
[property: CliArgument] string Secret
) : GcloudOptions
{
    [CliOption("--data-file")]
    public string? DataFile { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--set-annotations")]
    public IEnumerable<KeyValue>? SetAnnotations { get; set; }

    [CliOption("--topics")]
    public string[]? Topics { get; set; }

    [CliOption("--expire-time")]
    public string? ExpireTime { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliOption("--next-rotation-time")]
    public string? NextRotationTime { get; set; }

    [CliOption("--rotation-period")]
    public string? RotationPeriod { get; set; }

    [CliOption("--replication-policy-file")]
    public string? ReplicationPolicyFile { get; set; }

    [CliOption("--kms-key-name")]
    public string? KmsKeyName { get; set; }

    [CliOption("--locations")]
    public string[]? Locations { get; set; }

    [CliOption("--replication-policy")]
    public string? ReplicationPolicy { get; set; }
}