using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment", "source", "config")]
public record AzFunctionappDeploymentSourceConfigOptions(
[property: CommandSwitch("--repo-url")] string RepoUrl
) : AzOptions
{
    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--git-token")]
    public string? GitToken { get; set; }

    [CommandSwitch("--github-action")]
    public string? GithubAction { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--manual-integration")]
    public string? ManualIntegration { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--repository-type")]
    public string? RepositoryType { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

