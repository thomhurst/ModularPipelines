using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment", "configuration", "show")]
public record AzNginxDeploymentConfigurationShowOptions : AzOptions
{
    [CommandSwitch("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}