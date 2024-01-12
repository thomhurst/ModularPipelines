using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "apis", "get-iam-policy")]
public record GcloudApiGatewayApisGetIamPolicyOptions(
[property: PositionalArgument] string Api
) : GcloudOptions;