using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "gateways", "get-iam-policy")]
public record GcloudApiGatewayGatewaysGetIamPolicyOptions(
[property: PositionalArgument] string Gateway,
[property: PositionalArgument] string Location
) : GcloudOptions;