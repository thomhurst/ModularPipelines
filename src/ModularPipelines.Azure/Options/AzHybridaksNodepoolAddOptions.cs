using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hybridaks", "nodepool", "add")]
public record AzHybridaksNodepoolAddOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}