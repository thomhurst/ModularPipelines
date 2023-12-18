using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "log", "deployment", "show")]
public record AzWebappLogDeploymentShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--deployment-id")]
    public string? DeploymentId { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}