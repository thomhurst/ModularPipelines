using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "remove-iam-policy-binding")]
public record GcloudNetworkConnectivityHubsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Hub,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;