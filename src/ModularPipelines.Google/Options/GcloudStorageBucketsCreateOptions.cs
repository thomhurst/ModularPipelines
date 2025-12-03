using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "create")]
public record GcloudStorageBucketsCreateOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliOption("--default-encryption-key")]
    public string? DefaultEncryptionKey { get; set; }

    [CliOption("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [CliFlag("--enable-per-object-retention")]
    public bool? EnablePerObjectRetention { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--[no-]public-access-prevention")]
    public string[]? NoPublicAccessPrevention { get; set; }

    [CliOption("--placement")]
    public string? Placement { get; set; }

    [CliOption("--recovery-point-objective")]
    public string? RecoveryPointObjective { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--soft-delete-duration")]
    public string? SoftDeleteDuration { get; set; }

    [CliOption("--[no-]uniform-bucket-level-access")]
    public string[]? NoUniformBucketLevelAccess { get; set; }
}