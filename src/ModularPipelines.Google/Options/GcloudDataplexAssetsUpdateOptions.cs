using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "assets", "update")]
public record GcloudDataplexAssetsUpdateOptions(
[property: CliArgument] string Asset,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
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

    [CliOption("--resource-read-access-mode")]
    public string? ResourceReadAccessMode { get; set; }

    [CliFlag("DIRECT")]
    public bool? Direct { get; set; }

    [CliFlag("MANAGED")]
    public bool? Managed { get; set; }

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