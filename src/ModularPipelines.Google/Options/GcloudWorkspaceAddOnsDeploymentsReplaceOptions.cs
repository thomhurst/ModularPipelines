using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "replace")]
public record GcloudWorkspaceAddOnsDeploymentsReplaceOptions(
[property: PositionalArgument] string Deployment,
[property: CommandSwitch("--deployment-file")] string DeploymentFile,
[property: CommandSwitch("--deployment-object")] string DeploymentObject
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}