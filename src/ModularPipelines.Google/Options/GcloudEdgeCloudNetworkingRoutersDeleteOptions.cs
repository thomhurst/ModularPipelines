using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cloud", "networking", "routers", "delete")]
public record GcloudEdgeCloudNetworkingRoutersDeleteOptions(
[property: PositionalArgument] string Router,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}