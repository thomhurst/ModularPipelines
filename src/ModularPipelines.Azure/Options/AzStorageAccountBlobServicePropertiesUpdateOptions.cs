using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "blob-service-properties", "update")]
public record AzStorageAccountBlobServicePropertiesUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--change-feed-days")]
    public string? ChangeFeedDays { get; set; }

    [CommandSwitch("--container-days")]
    public string? ContainerDays { get; set; }

    [BooleanCommandSwitch("--container-retention")]
    public bool? ContainerRetention { get; set; }

    [CommandSwitch("--default-service-version")]
    public string? DefaultServiceVersion { get; set; }

    [CommandSwitch("--delete-retention-days")]
    public string? DeleteRetentionDays { get; set; }

    [BooleanCommandSwitch("--enable-change-feed")]
    public bool? EnableChangeFeed { get; set; }

    [BooleanCommandSwitch("--enable-delete-retention")]
    public bool? EnableDeleteRetention { get; set; }

    [BooleanCommandSwitch("--enable-last-access-tracking")]
    public bool? EnableLastAccessTracking { get; set; }

    [BooleanCommandSwitch("--enable-restore-policy")]
    public bool? EnableRestorePolicy { get; set; }

    [BooleanCommandSwitch("--enable-versioning")]
    public bool? EnableVersioning { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--restore-days")]
    public string? RestoreDays { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}