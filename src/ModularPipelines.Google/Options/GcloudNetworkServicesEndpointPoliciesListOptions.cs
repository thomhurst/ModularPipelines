using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "endpoint-policies", "list")]
public record GcloudNetworkServicesEndpointPoliciesListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;