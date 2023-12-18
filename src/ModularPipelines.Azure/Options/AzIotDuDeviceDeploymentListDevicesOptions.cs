using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "deployment", "list-devices")]
public record AzIotDuDeviceDeploymentListDevicesOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cid")] string Cid,
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--gid")] string Gid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}