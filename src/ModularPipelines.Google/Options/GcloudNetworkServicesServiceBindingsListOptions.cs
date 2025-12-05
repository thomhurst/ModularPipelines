using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "service-bindings", "list")]
public record GcloudNetworkServicesServiceBindingsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;