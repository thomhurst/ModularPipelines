using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "http-routes", "list")]
public record GcloudNetworkServicesHttpRoutesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;