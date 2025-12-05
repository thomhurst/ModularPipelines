using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "gateways", "get-iam-policy")]
public record GcloudApiGatewayGatewaysGetIamPolicyOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location
) : GcloudOptions;