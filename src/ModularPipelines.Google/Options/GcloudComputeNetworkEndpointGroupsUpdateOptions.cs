using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-endpoint-groups", "update")]
public record GcloudComputeNetworkEndpointGroupsUpdateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--add-endpoint")] string[] AddEndpoint,
[property: CommandSwitch("--remove-endpoint")] string[] RemoveEndpoint
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}