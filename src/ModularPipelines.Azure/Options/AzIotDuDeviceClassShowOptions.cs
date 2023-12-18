using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "class", "show")]
public record AzIotDuDeviceClassShowOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cid")] string Cid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [BooleanCommandSwitch("--best-update")]
    public bool? BestUpdate { get; set; }

    [CommandSwitch("--gid")]
    public string? Gid { get; set; }

    [BooleanCommandSwitch("--installable-updates")]
    public bool? InstallableUpdates { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--update-compliance")]
    public bool? UpdateCompliance { get; set; }
}

