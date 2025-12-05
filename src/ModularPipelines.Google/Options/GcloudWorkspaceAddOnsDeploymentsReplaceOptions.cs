using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "deployments", "replace")]
public record GcloudWorkspaceAddOnsDeploymentsReplaceOptions(
[property: CliArgument] string Deployment,
[property: CliOption("--deployment-file")] string DeploymentFile,
[property: CliOption("--deployment-object")] string DeploymentObject
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}