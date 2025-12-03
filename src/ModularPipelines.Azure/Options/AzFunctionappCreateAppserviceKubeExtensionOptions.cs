using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "create", "(appservice-kube", "extension)")]
public record AzFunctionappCreateAppserviceKubeExtensionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
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

    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliOption("--deployment-container-image-name")]
    public string? DeploymentContainerImageName { get; set; }

    [CliOption("--deployment-local-git")]
    public string? DeploymentLocalGit { get; set; }

    [CliOption("--deployment-source-branch")]
    public string? DeploymentSourceBranch { get; set; }

    [CliOption("--deployment-source-url")]
    public string? DeploymentSourceUrl { get; set; }

    [CliFlag("--disable-app-insights")]
    public bool? DisableAppInsights { get; set; }

    [CliOption("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CliOption("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [CliOption("--functions-version")]
    public string? FunctionsVersion { get; set; }

    [CliOption("--max-worker-count")]
    public int? MaxWorkerCount { get; set; }

    [CliOption("--min-worker-count")]
    public int? MinWorkerCount { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}