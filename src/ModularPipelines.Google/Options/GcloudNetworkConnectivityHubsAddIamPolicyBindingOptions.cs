using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs", "add-iam-policy-binding")]
public record GcloudNetworkConnectivityHubsAddIamPolicyBindingOptions(
[property: CliArgument] string Hub,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;