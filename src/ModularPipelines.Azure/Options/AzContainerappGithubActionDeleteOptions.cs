using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "github-action", "delete")]
public record AzContainerappGithubActionDeleteOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--login-with-github")]
    public bool? LoginWithGithub { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}