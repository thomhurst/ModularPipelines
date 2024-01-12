using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "update")]
public record GcloudStorageBucketsUpdateOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--clear-soft-delete")]
    public bool? ClearSoftDelete { get; set; }

    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CommandSwitch("--[no-]default-event-based-hold")]
    public string[]? NoDefaultEventBasedHold { get; set; }

    [CommandSwitch("--default-storage-class")]
    public string? DefaultStorageClass { get; set; }

    [BooleanCommandSwitch("--lock-retention-period")]
    public bool? LockRetentionPeriod { get; set; }

    [CommandSwitch("--recovery-point-objective")]
    public string? RecoveryPointObjective { get; set; }

    [CommandSwitch("--[no-]requester-pays")]
    public string[]? NoRequesterPays { get; set; }

    [CommandSwitch("--soft-delete-duration")]
    public string? SoftDeleteDuration { get; set; }

    [CommandSwitch("--[no-]uniform-bucket-level-access")]
    public string[]? NoUniformBucketLevelAccess { get; set; }

    [CommandSwitch("--[no-]versioning")]
    public string[]? NoVersioning { get; set; }

    [CommandSwitch("--acl-file")]
    public string? AclFile { get; set; }

    [CommandSwitch("--add-acl-grant")]
    public string[]? AddAclGrant { get; set; }

    [CommandSwitch("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CommandSwitch("--remove-acl-grant")]
    public string? RemoveAclGrant { get; set; }

    [CommandSwitch("--add-default-object-acl-grant")]
    public string[]? AddDefaultObjectAclGrant { get; set; }

    [CommandSwitch("--default-object-acl-file")]
    public string? DefaultObjectAclFile { get; set; }

    [CommandSwitch("--predefined-default-object-acl")]
    public string? PredefinedDefaultObjectAcl { get; set; }

    [CommandSwitch("--remove-default-object-acl-grant")]
    public string? RemoveDefaultObjectAclGrant { get; set; }

    [BooleanCommandSwitch("--clear-cors")]
    public bool? ClearCors { get; set; }

    [CommandSwitch("--cors-file")]
    public string? CorsFile { get; set; }

    [BooleanCommandSwitch("--clear-default-encryption-key")]
    public bool? ClearDefaultEncryptionKey { get; set; }

    [CommandSwitch("--default-encryption-key")]
    public string? DefaultEncryptionKey { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--labels-file")]
    public string? LabelsFile { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--update-labels")]
    public string[]? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-lifecycle")]
    public bool? ClearLifecycle { get; set; }

    [CommandSwitch("--lifecycle-file")]
    public string? LifecycleFile { get; set; }

    [BooleanCommandSwitch("--clear-log-bucket")]
    public bool? ClearLogBucket { get; set; }

    [CommandSwitch("--log-bucket")]
    public string? LogBucket { get; set; }

    [BooleanCommandSwitch("--clear-log-object-prefix")]
    public bool? ClearLogObjectPrefix { get; set; }

    [CommandSwitch("--log-object-prefix")]
    public string? LogObjectPrefix { get; set; }

    [BooleanCommandSwitch("--clear-public-access-prevention")]
    public bool? ClearPublicAccessPrevention { get; set; }

    [CommandSwitch("--[no-]public-access-prevention")]
    public string[]? NoPublicAccessPrevention { get; set; }

    [BooleanCommandSwitch("--clear-retention-period")]
    public bool? ClearRetentionPeriod { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [BooleanCommandSwitch("--clear-web-error-page")]
    public bool? ClearWebErrorPage { get; set; }

    [CommandSwitch("--web-error-page")]
    public string? WebErrorPage { get; set; }

    [BooleanCommandSwitch("--clear-web-main-page-suffix")]
    public bool? ClearWebMainPageSuffix { get; set; }

    [CommandSwitch("--web-main-page-suffix")]
    public string? WebMainPageSuffix { get; set; }
}