using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-endpoint-groups", "list")]
public record GcloudComputeNetworkEndpointGroupsListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }
}