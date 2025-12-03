using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-endpoint-groups", "update")]
public record GcloudComputeNetworkEndpointGroupsUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--add-endpoint")] string[] AddEndpoint,
[property: CliOption("--remove-endpoint")] string[] RemoveEndpoint
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}