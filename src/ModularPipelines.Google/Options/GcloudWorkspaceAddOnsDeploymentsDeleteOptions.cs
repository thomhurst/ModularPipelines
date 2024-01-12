using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "delete")]
public record GcloudWorkspaceAddOnsDeploymentsDeleteOptions(
[property: PositionalArgument] string Deployment
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}