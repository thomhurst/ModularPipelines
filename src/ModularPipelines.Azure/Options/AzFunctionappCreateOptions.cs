using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "create")]
public record AzFunctionappCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--app-insights")]
    public string? AppInsights { get; set; }

    [CliOption("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--consumption-plan-location")]
    public string? ConsumptionPlanLocation { get; set; }

    [CliOption("--deployment-local-git")]
    public string? DeploymentLocalGit { get; set; }

    [CliOption("--deployment-source-branch")]
    public string? DeploymentSourceBranch { get; set; }

    [CliOption("--deployment-source-url")]
    public string? DeploymentSourceUrl { get; set; }

    [CliFlag("--disable-app-insights")]
    public bool? DisableAppInsights { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--functions-version")]
    public string? FunctionsVersion { get; set; }

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CliOption("--registry-server")]
    public string? RegistryServer { get; set; }

    [CliOption("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}