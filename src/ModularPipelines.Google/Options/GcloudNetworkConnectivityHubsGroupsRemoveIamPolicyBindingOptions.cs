using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "groups", "remove-iam-policy-binding")]
public record GcloudNetworkConnectivityHubsGroupsRemoveIamPolicyBindingOptions(
[property: CliArgument] string Group,
[property: CliArgument] string Hub,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;