using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "deployment", "github-actions", "remove")]
public record AzFunctionappDeploymentGithubActionsRemoveOptions(
[property: CliOption("--repo")] string Repo
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}