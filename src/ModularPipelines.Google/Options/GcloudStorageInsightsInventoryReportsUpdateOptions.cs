using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "insights", "inventory-reports", "update")]
public record GcloudStorageInsightsInventoryReportsUpdateOptions(
[property: PositionalArgument] string ReportConfig,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--schedule-repeats")]
    public string? ScheduleRepeats { get; set; }

    [CommandSwitch("--schedule-repeats-until")]
    public string? ScheduleRepeatsUntil { get; set; }

    [CommandSwitch("--schedule-starts")]
    public string? ScheduleStarts { get; set; }

    [CommandSwitch("--metadata-fields")]
    public string[]? MetadataFields { get; set; }

    [CommandSwitch("--add-metadata-fields")]
    public string[]? AddMetadataFields { get; set; }

    [CommandSwitch("--remove-metadata-fields")]
    public string[]? RemoveMetadataFields { get; set; }

    [BooleanCommandSwitch("--parquet")]
    public bool? Parquet { get; set; }

    [CommandSwitch("--csv-delimiter")]
    public string? CsvDelimiter { get; set; }

    [CommandSwitch("--[no-]csv-header")]
    public string[]? NoCsvHeader { get; set; }

    [CommandSwitch("--csv-separator")]
    public string? CsvSeparator { get; set; }
}