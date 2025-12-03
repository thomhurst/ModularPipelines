using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "deployments", "describe")]
public record GcloudWorkspaceAddOnsDeploymentsDescribeOptions(
[property: CliArgument] string Deployment
) : GcloudOptions;