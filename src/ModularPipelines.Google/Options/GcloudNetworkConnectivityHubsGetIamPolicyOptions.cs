using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "get-iam-policy")]
public record GcloudNetworkConnectivityHubsGetIamPolicyOptions(
[property: PositionalArgument] string Hub
) : GcloudOptions;