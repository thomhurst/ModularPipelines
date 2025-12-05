using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "unmanaged-disk", "attach")]
public record AzVmUnmanagedDiskAttachOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--caching")]
    public string? Caching { get; set; }

    [CliOption("--lun")]
    public string? Lun { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--new")]
    public bool? New { get; set; }

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--vhd-uri")]
    public string? VhdUri { get; set; }
}