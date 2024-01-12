using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "create", "data-quality")]
public record GcloudDataplexDatascansCreateDataQualityOptions(
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--data-quality-spec-file")] string DataQualitySpecFile,
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

    [CommandSwitch("--incremental-field")]
    public string? IncrementalField { get; set; }

    [CommandSwitch("--on-demand")]
    public string? OnDemand { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }
}