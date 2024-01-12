using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "tcp-routes", "list")]
public record GcloudNetworkServicesTcpRoutesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;