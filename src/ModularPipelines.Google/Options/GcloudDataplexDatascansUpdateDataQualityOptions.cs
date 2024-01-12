using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "update", "data-quality")]
public record GcloudDataplexDatascansUpdateDataQualityOptions(
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--data-quality-spec-file")]
    public string? DataQualitySpecFile { get; set; }

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

    [CommandSwitch("--on-demand")]
    public string? OnDemand { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }
}