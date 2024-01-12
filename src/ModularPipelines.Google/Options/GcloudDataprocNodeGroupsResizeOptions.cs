using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "node-groups", "resize")]
public record GcloudDataprocNodeGroupsResizeOptions(
[property: PositionalArgument] string NodeGroup,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--size")] string Size
) : GcloudOptions
{
    [CommandSwitch("--graceful-decommission-timeout")]
    public string? GracefulDecommissionTimeout { get; set; }
}