using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "set-iam-policy")]
public record GcloudNetworkConnectivityHubsSetIamPolicyOptions(
[property: CliArgument] string Hub,
[property: CliArgument] string PolicyFile
) : GcloudOptions;