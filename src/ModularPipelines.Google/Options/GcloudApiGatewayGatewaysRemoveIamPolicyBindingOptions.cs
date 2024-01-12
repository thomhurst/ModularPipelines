using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "gateways", "remove-iam-policy-binding")]
public record GcloudApiGatewayGatewaysRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Gateway,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;