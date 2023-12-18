using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "group", "show")]
public record AzIotDuDeviceGroupShowOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--gid")] string Gid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [BooleanCommandSwitch("--best-updates")]
    public bool? BestUpdates { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--update-compliance")]
    public bool? UpdateCompliance { get; set; }
}