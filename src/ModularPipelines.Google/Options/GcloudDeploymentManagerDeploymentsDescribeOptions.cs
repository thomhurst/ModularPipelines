using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "describe")]
public record GcloudDeploymentManagerDeploymentsDescribeOptions(
[property: PositionalArgument] string DeploymentName
) : GcloudOptions;