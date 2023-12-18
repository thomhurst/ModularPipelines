using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logicapp", "create")]
public record AzLogicappCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--app-insights")]
    public string? AppInsights { get; set; }

    [CommandSwitch("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

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

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--plan")]
    public string? Plan { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

