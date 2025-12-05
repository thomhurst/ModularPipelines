using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "manager", "list-deploy-status")]
public record AzNetworkManagerListDeployStatusOptions : AzOptions
{
    [CliOption("--deployment-types")]
    public string? DeploymentTypes { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--regions")]
    public string? Regions { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}