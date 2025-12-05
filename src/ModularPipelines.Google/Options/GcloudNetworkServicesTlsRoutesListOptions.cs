using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "tls-routes", "list")]
public record GcloudNetworkServicesTlsRoutesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;