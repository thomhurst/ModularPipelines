using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "apis", "get-iam-policy")]
public record GcloudApiGatewayApisGetIamPolicyOptions(
[property: CliArgument] string Api
) : GcloudOptions;