using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "create")]
public record AzIotHubCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CliFlag("--edr")]
    public bool? Edr { get; set; }

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

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--partition-count")]
    public int? PartitionCount { get; set; }

    [CliOption("--rd")]
    public string? Rd { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--unit")]
    public string? Unit { get; set; }
}