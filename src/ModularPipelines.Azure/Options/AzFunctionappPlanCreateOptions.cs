using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "plan", "create")]
public record AzFunctionappPlanCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliFlag("--is-linux")]
    public bool? IsLinux { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--max-burst")]
    public string? MaxBurst { get; set; }

    [CliOption("--min-instances")]
    public string? MinInstances { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}