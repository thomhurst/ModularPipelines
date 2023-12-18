using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "deployment", "delete")]
public record AzIotDuDeviceDeploymentDeleteOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--deployment-id")] string DeploymentId,
[property: CommandSwitch("--gid")] string Gid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--cid")]
    public string? Cid { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

