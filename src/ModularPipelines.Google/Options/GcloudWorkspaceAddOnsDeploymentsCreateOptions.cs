using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "create")]
public record GcloudWorkspaceAddOnsDeploymentsCreateOptions(
[property: PositionalArgument] string Deployment,
[property: CommandSwitch("--deployment-file")] string DeploymentFile,
[property: CommandSwitch("--deployment-object")] string DeploymentObject
) : GcloudOptions;