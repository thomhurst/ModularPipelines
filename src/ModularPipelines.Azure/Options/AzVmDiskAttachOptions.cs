using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "disk", "attach")]
public record AzVmDiskAttachOptions(
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--caching")]
    public string? Caching { get; set; }

    [CliOption("--disks")]
    public string? Disks { get; set; }

    [CliFlag("--enable-write-accelerator")]
    public bool? EnableWriteAccelerator { get; set; }

    [CliOption("--lun")]
    public string? Lun { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--new")]
    public bool? New { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}