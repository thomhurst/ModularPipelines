using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "hubs", "set-iam-policy")]
public record GcloudNetworkConnectivityHubsSetIamPolicyOptions(
[property: PositionalArgument] string Hub,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;