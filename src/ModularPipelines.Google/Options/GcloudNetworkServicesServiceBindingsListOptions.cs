using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "service-bindings", "list")]
public record GcloudNetworkServicesServiceBindingsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;