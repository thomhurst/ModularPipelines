using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "manifests", "list")]
public record GcloudDeploymentManagerManifestsListOptions(
[property: CommandSwitch("--deployment")] string Deployment
) : GcloudOptions
{
    [BooleanCommandSwitch("--simple-list")]
    public bool? SimpleList { get; set; }
}