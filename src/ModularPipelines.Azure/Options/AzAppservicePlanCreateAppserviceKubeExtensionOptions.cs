using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appservice", "plan", "create", "(appservice-kube", "extension)")]
public record AzAppservicePlanCreateAppserviceKubeExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-service-environment")]
    public string? AppServiceEnvironment { get; set; }

    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

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
}