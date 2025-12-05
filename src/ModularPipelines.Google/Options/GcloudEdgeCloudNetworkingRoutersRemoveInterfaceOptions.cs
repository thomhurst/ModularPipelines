using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "networking", "routers", "remove-interface")]
public record GcloudEdgeCloudNetworkingRoutersRemoveInterfaceOptions(
[property: CliArgument] string Router,
[property: CliArgument] string Location,
[property: CliArgument] string Zone,
[property: CliOption("--interface-name")] string InterfaceName,
[property: CliOption("--interface-names")] string[] InterfaceNames
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}