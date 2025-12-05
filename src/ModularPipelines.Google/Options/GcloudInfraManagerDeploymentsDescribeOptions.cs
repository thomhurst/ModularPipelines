using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "describe")]
public record GcloudInfraManagerDeploymentsDescribeOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions;