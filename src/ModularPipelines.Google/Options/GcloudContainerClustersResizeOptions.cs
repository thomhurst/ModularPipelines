using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "clusters", "resize")]
public record GcloudContainerClustersResizeOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--num-nodes")] string NumNodes,
[property: CommandSwitch("--size")] string Size
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--node-pool")]
    public string? NodePool { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}