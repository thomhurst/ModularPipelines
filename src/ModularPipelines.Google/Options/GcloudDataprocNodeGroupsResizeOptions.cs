using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "node-groups", "resize")]
public record GcloudDataprocNodeGroupsResizeOptions(
[property: CliArgument] string NodeGroup,
[property: CliArgument] string Cluster,
[property: CliArgument] string Region,
[property: CliOption("--size")] string Size
) : GcloudOptions
{
    [CliOption("--graceful-decommission-timeout")]
    public string? GracefulDecommissionTimeout { get; set; }
}