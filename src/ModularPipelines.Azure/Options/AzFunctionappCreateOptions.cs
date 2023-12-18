using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "create")]
public record AzFunctionappCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount
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

    [CommandSwitch("--deployment-local-git")]
    public string? DeploymentLocalGit { get; set; }

    [CommandSwitch("--deployment-source-branch")]
    public string? DeploymentSourceBranch { get; set; }

    [CommandSwitch("--deployment-source-url")]
    public string? DeploymentSourceUrl { get; set; }

    [BooleanCommandSwitch("--disable-app-insights")]
    public bool? DisableAppInsights { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--functions-version")]
    public string? FunctionsVersion { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CommandSwitch("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--plan")]
    public string? Plan { get; set; }

    [CommandSwitch("--registry-password")]
    public string? RegistryPassword { get; set; }

    [CommandSwitch("--registry-server")]
    public string? RegistryServer { get; set; }

    [CommandSwitch("--registry-username")]
    public string? RegistryUsername { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}