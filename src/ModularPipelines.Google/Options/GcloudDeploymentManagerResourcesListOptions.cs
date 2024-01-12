using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "resources", "list")]
public record GcloudDeploymentManagerResourcesListOptions : GcloudOptions
{
    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [BooleanCommandSwitch("--simple-list")]
    public bool? SimpleList { get; set; }
}