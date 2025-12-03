using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("urestackhci", "virtualmachine", "update")]
public record AzAzurestackhciVirtualmachineUpdateOptions : AzOptions
{
    [CliOption("--cpu-count")]
    public int? CpuCount { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--memory-mb")]
    public string? MemoryMb { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vhd-names")]
    public string? VhdNames { get; set; }

    [CliOption("--vnic-names")]
    public string? VnicNames { get; set; }
}