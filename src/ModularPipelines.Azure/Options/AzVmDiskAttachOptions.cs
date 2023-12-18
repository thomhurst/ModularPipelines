using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "disk", "attach")]
public record AzVmDiskAttachOptions(
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--caching")]
    public string? Caching { get; set; }

    [CommandSwitch("--disks")]
    public string? Disks { get; set; }

    [BooleanCommandSwitch("--enable-write-accelerator")]
    public bool? EnableWriteAccelerator { get; set; }

    [CommandSwitch("--lun")]
    public string? Lun { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--new")]
    public bool? New { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}