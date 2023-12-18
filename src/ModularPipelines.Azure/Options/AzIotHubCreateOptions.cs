using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "create")]
public record AzIotHubCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--c2d-max-delivery-count")]
    public int? C2dMaxDeliveryCount { get; set; }

    [CommandSwitch("--c2d-ttl")]
    public string? C2dTtl { get; set; }

    [BooleanCommandSwitch("--dds")]
    public bool? Dds { get; set; }

    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [BooleanCommandSwitch("--disable-module-sas")]
    public bool? DisableModuleSas { get; set; }

    [BooleanCommandSwitch("--edr")]
    public bool? Edr { get; set; }

    [CommandSwitch("--fc")]
    public string? Fc { get; set; }

    [CommandSwitch("--fcs")]
    public string? Fcs { get; set; }

    [CommandSwitch("--fd")]
    public string? Fd { get; set; }

    [CommandSwitch("--feedback-lock-duration")]
    public string? FeedbackLockDuration { get; set; }

    [CommandSwitch("--feedback-ttl")]
    public string? FeedbackTtl { get; set; }

    [CommandSwitch("--fileupload-notification-lock-duration")]
    public string? FileuploadNotificationLockDuration { get; set; }

    [CommandSwitch("--fileupload-notification-max-delivery-count")]
    public int? FileuploadNotificationMaxDeliveryCount { get; set; }

    [CommandSwitch("--fileupload-notification-ttl")]
    public string? FileuploadNotificationTtl { get; set; }

    [BooleanCommandSwitch("--fileupload-notifications")]
    public bool? FileuploadNotifications { get; set; }

    [CommandSwitch("--fileupload-sas-ttl")]
    public string? FileuploadSasTtl { get; set; }

    [CommandSwitch("--fileupload-storage-auth-type")]
    public string? FileuploadStorageAuthType { get; set; }

    [CommandSwitch("--fileupload-storage-identity")]
    public string? FileuploadStorageIdentity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--min-tls-version")]
    public string? MinTlsVersion { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--partition-count")]
    public int? PartitionCount { get; set; }

    [CommandSwitch("--rd")]
    public string? Rd { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }
}

