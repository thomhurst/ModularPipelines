using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "update")]
public record GcloudContainerVmwareClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--admin-users")]
    public string? AdminUsers { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--metal-lb-config-address-pools")]
    public string[]? MetalLbConfigAddressPools { get; set; }

    [CommandSwitch("--static-ip-config-ip-blocks")]
    public string[]? StaticIpConfigIpBlocks { get; set; }

    [CommandSwitch("--upgrade-policy")]
    public string[]? UpgradePolicy { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--add-annotations")]
    public string[]? AddAnnotations { get; set; }

    [CommandSwitch("--remove-annotations")]
    public string[]? RemoveAnnotations { get; set; }

    [CommandSwitch("--cpus")]
    public string? Cpus { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [BooleanCommandSwitch("--disable-auto-resize")]
    public bool? DisableAutoResize { get; set; }

    [BooleanCommandSwitch("--enable-auto-resize")]
    public bool? EnableAutoResize { get; set; }

    [BooleanCommandSwitch("--disable-aag-config")]
    public bool? DisableAagConfig { get; set; }

    [BooleanCommandSwitch("--enable-aag-config")]
    public bool? EnableAagConfig { get; set; }

    [BooleanCommandSwitch("--disable-auto-repair")]
    public bool? DisableAutoRepair { get; set; }

    [BooleanCommandSwitch("--enable-auto-repair")]
    public bool? EnableAutoRepair { get; set; }

    [BooleanCommandSwitch("--disable-vsphere-csi")]
    public bool? DisableVsphereCsi { get; set; }

    [BooleanCommandSwitch("--enable-vsphere-csi")]
    public bool? EnableVsphereCsi { get; set; }
}