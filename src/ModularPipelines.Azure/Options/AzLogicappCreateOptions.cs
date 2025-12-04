using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logicapp", "create")]
public record AzLogicappCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--app-insights")]
    public string? AppInsights { get; set; }

    [CliOption("--app-insights-key")]
    public string? AppInsightsKey { get; set; }

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

    [CliFlag("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CliOption("--plan")]
    public string? Plan { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}