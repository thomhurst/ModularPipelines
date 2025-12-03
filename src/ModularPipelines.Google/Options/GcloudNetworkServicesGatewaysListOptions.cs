using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "gateways", "list")]
public record GcloudNetworkServicesGatewaysListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;