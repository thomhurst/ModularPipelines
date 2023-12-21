using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "disk", "attach")]
public record AzVmssDiskAttachOptions : AzOptions
{
    [CommandSwitch("--caching")]
    public string? Caching { get; set; }

    [CommandSwitch("--disk")]
    public string? Disk { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--lun")]
    public string? Lun { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--vmss-name")]
    public string? VmssName { get; set; }
}