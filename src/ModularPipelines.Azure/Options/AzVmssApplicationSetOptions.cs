using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "application", "set")]
public record AzVmssApplicationSetOptions(
[property: CommandSwitch("--app-version-ids")] string AppVersionIds
) : AzOptions
{
    [CommandSwitch("--app-config-overrides")]
    public string? AppConfigOverrides { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--order-applications")]
    public bool? OrderApplications { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--treat-deployment-as-failure")]
    public string? TreatDeploymentAsFailure { get; set; }
}