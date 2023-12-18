using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "online-endpoint", "invoke")]
public record AzMlOnlineEndpointInvokeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [CommandSwitch("--request-file")]
    public string? RequestFile { get; set; }
}