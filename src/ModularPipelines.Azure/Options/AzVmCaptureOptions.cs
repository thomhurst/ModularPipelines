using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "capture")]
public record AzVmCaptureOptions(
[property: CommandSwitch("--vhd-name-prefix")] string VhdNamePrefix
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--storage-container")]
    public string? StorageContainer { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}