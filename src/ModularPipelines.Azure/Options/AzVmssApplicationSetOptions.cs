using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmss", "application", "set")]
public record AzVmssApplicationSetOptions(
[property: CliOption("--app-version-ids")] string AppVersionIds
) : AzOptions
{
    [CliOption("--app-config-overrides")]
    public string? AppConfigOverrides { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--order-applications")]
    public bool? OrderApplications { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--treat-deployment-as-failure")]
    public string? TreatDeploymentAsFailure { get; set; }
}