using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "hub", "update")]
public record AzIotHubUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

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

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--rd")]
    public string? Rd { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--unit")]
    public string? Unit { get; set; }
}