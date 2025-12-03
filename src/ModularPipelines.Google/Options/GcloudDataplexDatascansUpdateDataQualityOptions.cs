using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "update", "data-quality")]
public record GcloudDataplexDatascansUpdateDataQualityOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--data-quality-spec-file")]
    public string? DataQualitySpecFile { get; set; }

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

    [CliOption("--on-demand")]
    public string? OnDemand { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }
}