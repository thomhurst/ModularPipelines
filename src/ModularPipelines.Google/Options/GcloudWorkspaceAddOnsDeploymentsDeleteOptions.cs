using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "deployments", "delete")]
public record GcloudWorkspaceAddOnsDeploymentsDeleteOptions(
[property: CliArgument] string Deployment
) : GcloudOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }
}