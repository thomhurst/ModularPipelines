using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "apis", "remove-iam-policy-binding")]
public record GcloudApiGatewayApisRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Api,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;