using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "assets", "create")]
public record GcloudDataplexAssetsCreateOptions(
[property: CliArgument] string Asset,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliFlag("BIGQUERY_DATASET")] bool BigqueryDataset,
[property: CliFlag("STORAGE_BUCKET")] bool StorageBucket,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resource-read-access-mode")] string ResourceReadAccessMode,
[property: CliFlag("DIRECT")] bool Direct,
[property: CliFlag("MANAGED")] bool Managed
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--[no-]discovery-enabled")]
    public string[]? NoDiscoveryEnabled { get; set; }

    [CliOption("--discovery-exclude-patterns")]
    public string[]? DiscoveryExcludePatterns { get; set; }

    [CliOption("--discovery-include-patterns")]
    public string[]? DiscoveryIncludePatterns { get; set; }

    [CliOption("--discovery-schedule")]
    public string? DiscoverySchedule { get; set; }

    [CliOption("--csv-delimiter")]
    public string? CsvDelimiter { get; set; }

    [CliOption("--[no-]csv-disable-type-inference")]
    public string[]? NoCsvDisableTypeInference { get; set; }

    [CliOption("--csv-encoding")]
    public string? CsvEncoding { get; set; }

    [CliOption("--csv-header-rows")]
    public string? CsvHeaderRows { get; set; }

    [CliOption("--[no-]json-disable-type-inference")]
    public string[]? NoJsonDisableTypeInference { get; set; }

    [CliOption("--json-encoding")]
    public string? JsonEncoding { get; set; }
}