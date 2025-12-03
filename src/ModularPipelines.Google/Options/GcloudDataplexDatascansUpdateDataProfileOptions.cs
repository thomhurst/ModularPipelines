using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "update", "data-profile")]
public record GcloudDataplexDatascansUpdateDataProfileOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--data-profile-spec-file")]
    public string? DataProfileSpecFile { get; set; }

    [CliOption("--exclude-field-names")]
    public string? ExcludeFieldNames { get; set; }

    [CliOption("--include-field-names")]
    public string? IncludeFieldNames { get; set; }

    [CliOption("--row-filter")]
    public string? RowFilter { get; set; }

    [CliOption("--sampling-percent")]
    public string? SamplingPercent { get; set; }

    [CliOption("--on-demand")]
    public string? OnDemand { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }
}