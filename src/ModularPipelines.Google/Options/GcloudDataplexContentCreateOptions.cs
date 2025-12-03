using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "content", "create")]
public record GcloudDataplexContentCreateOptions(
[property: CliOption("--data-text")] string DataText,
[property: CliOption("--path")] string Path,
[property: CliOption("--kernel-type")] string KernelType,
[property: CliFlag("PYTHON3")] bool PYTHON3,
[property: CliOption("--query-engine")] string QueryEngine,
[property: CliFlag("SPARK")] bool Spark,
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}