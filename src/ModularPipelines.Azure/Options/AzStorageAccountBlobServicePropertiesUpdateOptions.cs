using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "blob-service-properties", "update")]
public record AzStorageAccountBlobServicePropertiesUpdateOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--change-feed-days")]
    public string? ChangeFeedDays { get; set; }

    [CliOption("--container-days")]
    public string? ContainerDays { get; set; }

    [CliFlag("--container-retention")]
    public bool? ContainerRetention { get; set; }

    [CliOption("--default-service-version")]
    public string? DefaultServiceVersion { get; set; }

    [CliOption("--delete-retention-days")]
    public string? DeleteRetentionDays { get; set; }

    [CliFlag("--enable-change-feed")]
    public bool? EnableChangeFeed { get; set; }

    [CliFlag("--enable-delete-retention")]
    public bool? EnableDeleteRetention { get; set; }

    [CliFlag("--enable-last-access-tracking")]
    public bool? EnableLastAccessTracking { get; set; }

    [CliFlag("--enable-restore-policy")]
    public bool? EnableRestorePolicy { get; set; }

    [CliFlag("--enable-versioning")]
    public bool? EnableVersioning { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--restore-days")]
    public string? RestoreDays { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}