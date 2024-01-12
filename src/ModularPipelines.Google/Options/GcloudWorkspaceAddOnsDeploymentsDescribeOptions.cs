using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "describe")]
public record GcloudWorkspaceAddOnsDeploymentsDescribeOptions(
[property: PositionalArgument] string Deployment
) : GcloudOptions;