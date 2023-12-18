using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "deployment", "list")]
public record AzIotDuDeviceDeploymentListOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--gid")] string Gid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--cid")]
    public string? Cid { get; set; }

    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

