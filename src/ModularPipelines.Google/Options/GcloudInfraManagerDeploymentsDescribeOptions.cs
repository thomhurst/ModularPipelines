using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "describe")]
public record GcloudInfraManagerDeploymentsDescribeOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions;