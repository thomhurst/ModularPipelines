using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "deployment", "source", "config")]
public record AzWebappDeploymentSourceConfigOptions(
[property: CliOption("--repo-url")] string RepoUrl
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--git-token")]
    public string? GitToken { get; set; }

    [CliOption("--github-action")]
    public string? GithubAction { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--manual-integration")]
    public string? ManualIntegration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--repository-type")]
    public string? RepositoryType { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}