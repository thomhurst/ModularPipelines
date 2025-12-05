using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "apis", "add-iam-policy-binding")]
public record GcloudApiGatewayApisAddIamPolicyBindingOptions(
[property: CliArgument] string Api,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;