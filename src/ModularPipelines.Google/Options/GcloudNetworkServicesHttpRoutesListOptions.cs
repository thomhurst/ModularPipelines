using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "http-routes", "list")]
public record GcloudNetworkServicesHttpRoutesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;