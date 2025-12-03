using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "create", "data-quality")]
public record GcloudDataplexDatascansCreateDataQualityOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location,
[property: CliOption("--data-quality-spec-file")] string DataQualitySpecFile,
[property: CliOption("--data-source-entity")] string DataSourceEntity,
[property: CliOption("--data-source-resource")] string DataSourceResource
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

    [CliOption("--incremental-field")]
    public string? IncrementalField { get; set; }

    [CliOption("--on-demand")]
    public string? OnDemand { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }
}