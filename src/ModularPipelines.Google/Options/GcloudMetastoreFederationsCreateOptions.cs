using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "federations", "create")]
public record GcloudMetastoreFederationsCreateOptions(
[property: CliArgument] string Federation,
[property: CliArgument] string Location,
[property: CliOption("--backends")] string Backends
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--hive-metastore-version")]
    public string? HiveMetastoreVersion { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}