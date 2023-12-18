using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "class", "list")]
public record AzIotDuDeviceClassListOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--gid")]
    public string? Gid { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}