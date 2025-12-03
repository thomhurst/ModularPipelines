using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appservice", "plan", "create")]
public record AzAppservicePlanCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-service-environment")]
    public string? AppServiceEnvironment { get; set; }

    [CliOption("--hyper-v")]
    public string? HyperV { get; set; }

    [CliOption("--is-linux")]
    public string? IsLinux { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--number-of-workers")]
    public string? NumberOfWorkers { get; set; }

    [CliFlag("--per-site-scaling")]
    public bool? PerSiteScaling { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}