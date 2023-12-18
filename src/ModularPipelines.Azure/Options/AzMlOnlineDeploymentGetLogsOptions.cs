using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "online-deployment", "get-logs")]
public record AzMlOnlineDeploymentGetLogsOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--lines")]
    public string? Lines { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }
}