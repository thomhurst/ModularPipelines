using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "restorable-time-range", "find")]
public record AzDataprotectionRestorableTimeRangeFindOptions(
[property: CliOption("--source-data-store-type")] string SourceDataStoreType
) : AzOptions
{
    [CliOption("--backup-instance-name")]
    public string? BackupInstanceName { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}