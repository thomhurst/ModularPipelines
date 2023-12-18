using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "plan", "create")]
public record AzAppservicePlanCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--app-service-environment")]
    public string? AppServiceEnvironment { get; set; }

    [CommandSwitch("--hyper-v")]
    public string? HyperV { get; set; }

    [CommandSwitch("--is-linux")]
    public string? IsLinux { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--number-of-workers")]
    public string? NumberOfWorkers { get; set; }

    [BooleanCommandSwitch("--per-site-scaling")]
    public bool? PerSiteScaling { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}