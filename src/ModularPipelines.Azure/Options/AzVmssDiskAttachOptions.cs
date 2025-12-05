using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "disk", "attach")]
public record AzVmssDiskAttachOptions : AzOptions
{
    [CliOption("--caching")]
    public string? Caching { get; set; }

    [CliOption("--disk")]
    public string? Disk { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--lun")]
    public string? Lun { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vmss-name")]
    public string? VmssName { get; set; }
}