using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "tls-routes", "list")]
public record GcloudNetworkServicesTlsRoutesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;