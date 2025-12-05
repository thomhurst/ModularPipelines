using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "clusters", "update")]
public record GcloudContainerVmwareClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--admin-users")]
    public string? AdminUsers { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--metal-lb-config-address-pools")]
    public string[]? MetalLbConfigAddressPools { get; set; }

    [CliOption("--static-ip-config-ip-blocks")]
    public string[]? StaticIpConfigIpBlocks { get; set; }

    [CliOption("--upgrade-policy")]
    public string[]? UpgradePolicy { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--add-annotations")]
    public string[]? AddAnnotations { get; set; }

    [CliOption("--remove-annotations")]
    public string[]? RemoveAnnotations { get; set; }

    [CliOption("--cpus")]
    public string? Cpus { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliFlag("--disable-auto-resize")]
    public bool? DisableAutoResize { get; set; }

    [CliFlag("--enable-auto-resize")]
    public bool? EnableAutoResize { get; set; }

    [CliFlag("--disable-aag-config")]
    public bool? DisableAagConfig { get; set; }

    [CliFlag("--enable-aag-config")]
    public bool? EnableAagConfig { get; set; }

    [CliFlag("--disable-auto-repair")]
    public bool? DisableAutoRepair { get; set; }

    [CliFlag("--enable-auto-repair")]
    public bool? EnableAutoRepair { get; set; }

    [CliFlag("--disable-vsphere-csi")]
    public bool? DisableVsphereCsi { get; set; }

    [CliFlag("--enable-vsphere-csi")]
    public bool? EnableVsphereCsi { get; set; }
}