using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "clusters", "delete")]
public record GcloudAlloydbClustersDeleteOptions(
[property: CliArgument] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}