using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "gateways", "remove-iam-policy-binding")]
public record GcloudApiGatewayGatewaysRemoveIamPolicyBindingOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;