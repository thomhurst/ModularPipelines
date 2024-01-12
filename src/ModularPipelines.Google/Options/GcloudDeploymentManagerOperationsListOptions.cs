using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "operations", "list")]
public record GcloudDeploymentManagerOperationsListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--simple-list")]
    public bool? SimpleList { get; set; }
}