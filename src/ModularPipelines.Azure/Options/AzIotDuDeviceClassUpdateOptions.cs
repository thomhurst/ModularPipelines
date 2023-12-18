using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "class", "update")]
public record AzIotDuDeviceClassUpdateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cid")] string Cid,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}