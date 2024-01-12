using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "gateways", "list")]
public record GcloudNetworkServicesGatewaysListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;