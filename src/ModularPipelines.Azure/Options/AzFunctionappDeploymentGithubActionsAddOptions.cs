using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "deployment", "github-actions", "add")]
public record AzFunctionappDeploymentGithubActionsAddOptions(
[property: CliOption("--repo")] string Repo
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--build-path")]
    public string? BuildPath { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}