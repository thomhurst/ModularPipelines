using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "get-iam-policy")]
public record GcloudNetworkConnectivityHubsGetIamPolicyOptions(
[property: CliArgument] string Hub
) : GcloudOptions;