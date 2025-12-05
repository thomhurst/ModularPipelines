using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "insights", "inventory-reports", "update")]
public record GcloudStorageInsightsInventoryReportsUpdateOptions(
[property: CliArgument] string ReportConfig,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--schedule-repeats")]
    public string? ScheduleRepeats { get; set; }

    [CliOption("--schedule-repeats-until")]
    public string? ScheduleRepeatsUntil { get; set; }

    [CliOption("--schedule-starts")]
    public string? ScheduleStarts { get; set; }

    [CliOption("--metadata-fields")]
    public string[]? MetadataFields { get; set; }

    [CliOption("--add-metadata-fields")]
    public string[]? AddMetadataFields { get; set; }

    [CliOption("--remove-metadata-fields")]
    public string[]? RemoveMetadataFields { get; set; }

    [CliFlag("--parquet")]
    public bool? Parquet { get; set; }

    [CliOption("--csv-delimiter")]
    public string? CsvDelimiter { get; set; }

    [CliOption("--[no-]csv-header")]
    public string[]? NoCsvHeader { get; set; }

    [CliOption("--csv-separator")]
    public string? CsvSeparator { get; set; }
}