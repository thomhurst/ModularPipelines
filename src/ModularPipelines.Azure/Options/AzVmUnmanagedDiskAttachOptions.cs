using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "unmanaged-disk", "attach")]
public record AzVmUnmanagedDiskAttachOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--caching")]
    public string? Caching { get; set; }

    [CommandSwitch("--lun")]
    public string? Lun { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--new")]
    public bool? New { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--vhd-uri")]
    public string? VhdUri { get; set; }
}