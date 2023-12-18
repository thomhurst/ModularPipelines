using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "edge", "deployment", "update")]
public record AzIotEdgeDeploymentUpdateOptions(
[property: CommandSwitch("--deployment-id")] string DeploymentId
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--hub-name")]
    public string? HubName { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }
}