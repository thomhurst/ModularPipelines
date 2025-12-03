using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "export")]
public record GcloudDataprocClustersExportOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }
}