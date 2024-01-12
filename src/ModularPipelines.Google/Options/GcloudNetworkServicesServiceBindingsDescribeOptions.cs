using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "service-bindings", "describe")]
public record GcloudNetworkServicesServiceBindingsDescribeOptions(
[property: PositionalArgument] string ServiceBinding,
[property: PositionalArgument] string Location
) : GcloudOptions;