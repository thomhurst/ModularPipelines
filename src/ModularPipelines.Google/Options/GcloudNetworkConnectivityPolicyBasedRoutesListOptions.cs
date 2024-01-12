using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "policy-based-routes", "list")]
public record GcloudNetworkConnectivityPolicyBasedRoutesListOptions : GcloudOptions;