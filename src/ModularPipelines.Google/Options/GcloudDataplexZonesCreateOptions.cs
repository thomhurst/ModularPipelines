using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "zones", "create")]
public record GcloudDataplexZonesCreateOptions(
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--resource-location-type")] string ResourceLocationType,
[property: BooleanCommandSwitch("MULTI_REGION")] bool MultiRegion,
[property: BooleanCommandSwitch("SINGLE_REGION")] bool SingleRegion,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--[no-]discovery-enabled")]
    public string[]? NoDiscoveryEnabled { get; set; }

    [CommandSwitch("--discovery-exclude-patterns")]
    public string[]? DiscoveryExcludePatterns { get; set; }

    [CommandSwitch("--discovery-include-patterns")]
    public string[]? DiscoveryIncludePatterns { get; set; }

    [CommandSwitch("--discovery-schedule")]
    public string? DiscoverySchedule { get; set; }

    [CommandSwitch("--csv-delimiter")]
    public string? CsvDelimiter { get; set; }

    [CommandSwitch("--[no-]csv-disable-type-inference")]
    public string[]? NoCsvDisableTypeInference { get; set; }

    [CommandSwitch("--csv-encoding")]
    public string? CsvEncoding { get; set; }

    [CommandSwitch("--csv-header-rows")]
    public string? CsvHeaderRows { get; set; }

    [CommandSwitch("--[no-]json-disable-type-inference")]
    public string[]? NoJsonDisableTypeInference { get; set; }

    [CommandSwitch("--json-encoding")]
    public string? JsonEncoding { get; set; }
}