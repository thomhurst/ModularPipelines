using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "create", "(appservice-kube", "extension)")]
public record AzFunctionappCreateAppserviceKubeExtensionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--app-insights")]
    public string? AppInsights { get; set; }

    [CommandSwitch("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--consumption-plan-location")]
    public string? ConsumptionPlanLocation { get; set; }

    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [CommandSwitch("--deployment-container-image-name")]
    public string? DeploymentContainerImageName { get; set; }

    [CommandSwitch("--deployment-local-git")]
    public string? DeploymentLocalGit { get; set; }

    [CommandSwitch("--deployment-source-branch")]
    public string? DeploymentSourceBranch { get; set; }

    [CommandSwitch("--deployment-source-url")]
    public string? DeploymentSourceUrl { get; set; }

    [BooleanCommandSwitch("--disable-app-insights")]
    public bool? DisableAppInsights { get; set; }

    [CommandSwitch("--docker-registry-server-password")]
    public string? DockerRegistryServerPassword { get; set; }

    [CommandSwitch("--docker-registry-server-user")]
    public string? DockerRegistryServerUser { get; set; }

    [CommandSwitch("--functions-version")]
    public string? FunctionsVersion { get; set; }

    [CommandSwitch("--max-worker-count")]
    public int? MaxWorkerCount { get; set; }

    [CommandSwitch("--min-worker-count")]
    public int? MinWorkerCount { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--plan")]
    public string? Plan { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}