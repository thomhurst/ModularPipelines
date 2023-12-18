using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "deployment", "cancel")]
public record AzIotDuDeviceDeploymentCancelOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cid")] string Cid,
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--gid")] string Gid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}