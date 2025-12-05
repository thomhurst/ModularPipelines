using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "resize")]
public record GcloudContainerClustersResizeOptions(
[property: CliArgument] string Name,
[property: CliOption("--num-nodes")] string NumNodes,
[property: CliOption("--size")] string Size
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--node-pool")]
    public string? NodePool { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}