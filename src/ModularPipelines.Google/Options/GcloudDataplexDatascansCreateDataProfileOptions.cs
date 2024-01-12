using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "create", "data-profile")]
public record GcloudDataplexDatascansCreateDataProfileOptions(
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--data-source-entity")] string DataSourceEntity,
[property: CommandSwitch("--data-source-resource")] string DataSourceResource
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--data-profile-spec-file")]
    public string? DataProfileSpecFile { get; set; }

    [CommandSwitch("--exclude-field-names")]
    public string? ExcludeFieldNames { get; set; }

    [CommandSwitch("--export-results-table")]
    public string? ExportResultsTable { get; set; }

    [CommandSwitch("--include-field-names")]
    public string? IncludeFieldNames { get; set; }

    [CommandSwitch("--row-filter")]
    public string? RowFilter { get; set; }

    [CommandSwitch("--sampling-percent")]
    public string? SamplingPercent { get; set; }

    [CommandSwitch("--incremental-field")]
    public string? IncrementalField { get; set; }

    [CommandSwitch("--on-demand")]
    public string? OnDemand { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }
}