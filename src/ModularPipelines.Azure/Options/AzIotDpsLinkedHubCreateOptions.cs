using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "linked-hub", "create")]
public record AzIotDpsLinkedHubCreateOptions(
[property: CliOption("--dps-name")] string DpsName
) : AzOptions
{
    [CliOption("--allocation-weight")]
    public string? AllocationWeight { get; set; }

    [CliFlag("--apply-allocation-policy")]
    public bool? ApplyAllocationPolicy { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--hn")]
    public string? Hn { get; set; }

    [CliOption("--hrg")]
    public string? Hrg { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}