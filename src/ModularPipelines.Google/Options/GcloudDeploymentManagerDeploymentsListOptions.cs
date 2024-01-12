using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "list")]
public record GcloudDeploymentManagerDeploymentsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--simple-list")]
    public bool? SimpleList { get; set; }
}