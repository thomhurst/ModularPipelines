using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scvmm", "vm", "update")]
public record AzScvmmVmUpdateOptions : AzOptions
{
    [CliOption("--availability-sets")]
    public string? AvailabilitySets { get; set; }

    [CliOption("--cpu-count")]
    public int? CpuCount { get; set; }

    [CliFlag("--dynamic-memory-enabled")]
    public bool? DynamicMemoryEnabled { get; set; }

    [CliOption("--dynamic-memory-max")]
    public string? DynamicMemoryMax { get; set; }

    [CliOption("--dynamic-memory-min")]
    public string? DynamicMemoryMin { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--memory-size")]
    public string? MemorySize { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}