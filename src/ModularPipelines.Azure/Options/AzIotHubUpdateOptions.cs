using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "update")]
public record AzIotHubUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--c2d-max-delivery-count")]
    public int? C2dMaxDeliveryCount { get; set; }

    [CliOption("--c2d-ttl")]
    public string? C2dTtl { get; set; }

    [CliFlag("--dds")]
    public bool? Dds { get; set; }

    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliFlag("--disable-module-sas")]
    public bool? DisableModuleSas { get; set; }

    [CliOption("--fc")]
    public string? Fc { get; set; }

    [CliOption("--fcs")]
    public string? Fcs { get; set; }

    [CliOption("--fd")]
    public string? Fd { get; set; }

    [CliOption("--feedback-lock-duration")]
    public string? FeedbackLockDuration { get; set; }

    [CliOption("--feedback-ttl")]
    public string? FeedbackTtl { get; set; }

    [CliOption("--fileupload-notification-lock-duration")]
    public string? FileuploadNotificationLockDuration { get; set; }

    [CliOption("--fileupload-notification-max-delivery-count")]
    public int? FileuploadNotificationMaxDeliveryCount { get; set; }

    [CliOption("--fileupload-notification-ttl")]
    public string? FileuploadNotificationTtl { get; set; }

    [CliFlag("--fileupload-notifications")]
    public bool? FileuploadNotifications { get; set; }

    [CliOption("--fileupload-sas-ttl")]
    public string? FileuploadSasTtl { get; set; }

    [CliOption("--fileupload-storage-auth-type")]
    public string? FileuploadStorageAuthType { get; set; }

    [CliOption("--fileupload-storage-identity")]
    public string? FileuploadStorageIdentity { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--rd")]
    public string? Rd { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }
}