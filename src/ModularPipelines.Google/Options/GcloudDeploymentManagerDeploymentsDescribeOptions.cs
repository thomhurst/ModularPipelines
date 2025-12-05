using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "deployments", "describe")]
public record GcloudDeploymentManagerDeploymentsDescribeOptions(
[property: CliArgument] string DeploymentName
) : GcloudOptions;