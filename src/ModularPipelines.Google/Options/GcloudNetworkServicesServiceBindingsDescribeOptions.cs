using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "service-bindings", "describe")]
public record GcloudNetworkServicesServiceBindingsDescribeOptions(
[property: CliArgument] string ServiceBinding,
[property: CliArgument] string Location
) : GcloudOptions;