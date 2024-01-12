using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "create")]
public record GcloudStorageBucketsCreateOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CommandSwitch("--default-encryption-key")]
    public string? DefaultEncryptionKey { get; set; }

    [CommandSwitch("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [BooleanCommandSwitch("--enable-per-object-retention")]
    public bool? EnablePerObjectRetention { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--[no-]public-access-prevention")]
    public string[]? NoPublicAccessPrevention { get; set; }

    [CommandSwitch("--placement")]
    public string? Placement { get; set; }

    [CommandSwitch("--recovery-point-objective")]
    public string? RecoveryPointObjective { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--soft-delete-duration")]
    public string? SoftDeleteDuration { get; set; }

    [CommandSwitch("--[no-]uniform-bucket-level-access")]
    public string[]? NoUniformBucketLevelAccess { get; set; }
}