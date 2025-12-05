using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "tcp-routes", "list")]
public record GcloudNetworkServicesTcpRoutesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;