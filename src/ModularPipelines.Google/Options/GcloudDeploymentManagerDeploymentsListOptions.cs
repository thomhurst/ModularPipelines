using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "deployments", "list")]
public record GcloudDeploymentManagerDeploymentsListOptions : GcloudOptions
{
    [CliFlag("--simple-list")]
    public bool? SimpleList { get; set; }
}