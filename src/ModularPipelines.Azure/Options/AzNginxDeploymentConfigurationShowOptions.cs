using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nginx", "deployment", "configuration", "show")]
public record AzNginxDeploymentConfigurationShowOptions : AzOptions
{
    [CliOption("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}