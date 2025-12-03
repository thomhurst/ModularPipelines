using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "update")]
public record GcloudStorageBucketsUpdateOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--clear-soft-delete")]
    public bool? ClearSoftDelete { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliOption("--[no-]default-event-based-hold")]
    public string[]? NoDefaultEventBasedHold { get; set; }

    [CliOption("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [CliFlag("--lock-retention-period")]
    public bool? LockRetentionPeriod { get; set; }

    [CliOption("--recovery-point-objective")]
    public string? RecoveryPointObjective { get; set; }

    [CliOption("--[no-]requester-pays")]
    public string[]? NoRequesterPays { get; set; }

    [CliOption("--soft-delete-duration")]
    public string? SoftDeleteDuration { get; set; }

    [CliOption("--[no-]uniform-bucket-level-access")]
    public string[]? NoUniformBucketLevelAccess { get; set; }

    [CliOption("--[no-]versioning")]
    public string[]? NoVersioning { get; set; }

    [CliOption("--acl-file")]
    public string? AclFile { get; set; }

    [CliOption("--add-acl-grant")]
    public string[]? AddAclGrant { get; set; }

    [CliOption("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CliOption("--remove-acl-grant")]
    public string? RemoveAclGrant { get; set; }

    [CliOption("--add-default-object-acl-grant")]
    public string[]? AddDefaultObjectAclGrant { get; set; }

    [CliOption("--default-object-acl-file")]
    public string? DefaultObjectAclFile { get; set; }

    [CliOption("--predefined-default-object-acl")]
    public string? PredefinedDefaultObjectAcl { get; set; }

    [CliOption("--remove-default-object-acl-grant")]
    public string? RemoveDefaultObjectAclGrant { get; set; }

    [CliFlag("--clear-cors")]
    public bool? ClearCors { get; set; }

    [CliOption("--cors-file")]
    public string? CorsFile { get; set; }

    [CliFlag("--clear-default-encryption-key")]
    public bool? ClearDefaultEncryptionKey { get; set; }

    [CliOption("--default-encryption-key")]
    public string? DefaultEncryptionKey { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--labels-file")]
    public string? LabelsFile { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--update-labels")]
    public string[]? UpdateLabels { get; set; }

    [CliFlag("--clear-lifecycle")]
    public bool? ClearLifecycle { get; set; }

    [CliOption("--lifecycle-file")]
    public string? LifecycleFile { get; set; }

    [CliFlag("--clear-log-bucket")]
    public bool? ClearLogBucket { get; set; }

    [CliOption("--log-bucket")]
    public string? LogBucket { get; set; }

    [CliFlag("--clear-log-object-prefix")]
    public bool? ClearLogObjectPrefix { get; set; }

    [CliOption("--log-object-prefix")]
    public string? LogObjectPrefix { get; set; }

    [CliFlag("--clear-public-access-prevention")]
    public bool? ClearPublicAccessPrevention { get; set; }

    [CliOption("--[no-]public-access-prevention")]
    public string[]? NoPublicAccessPrevention { get; set; }

    [CliFlag("--clear-retention-period")]
    public bool? ClearRetentionPeriod { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliFlag("--clear-web-error-page")]
    public bool? ClearWebErrorPage { get; set; }

    [CliOption("--web-error-page")]
    public string? WebErrorPage { get; set; }

    [CliFlag("--clear-web-main-page-suffix")]
    public bool? ClearWebMainPageSuffix { get; set; }

    [CliOption("--web-main-page-suffix")]
    public string? WebMainPageSuffix { get; set; }
}