using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "service-connection-policies", "list")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;