using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "linked-hub", "update")]
public record AzIotDpsLinkedHubUpdateOptions(
[property: CliOption("--dps-name")] string DpsName,
[property: CliOption("--linked-hub")] string LinkedHub
) : AzOptions
{
    [CliOption("--allocation-weight")]
    public string? AllocationWeight { get; set; }

    [CliFlag("--apply-allocation-policy")]
    public bool? ApplyAllocationPolicy { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}