using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "resources", "describe")]
public record GcloudDeploymentManagerResourcesDescribeOptions(
[property: PositionalArgument] string Resource
) : GcloudOptions
{
    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }
}