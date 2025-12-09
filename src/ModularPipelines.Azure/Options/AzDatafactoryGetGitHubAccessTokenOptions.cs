using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "get-git-hub-access-token")]
public record AzDatafactoryGetGitHubAccessTokenOptions(
[property: CliOption("--git-hub-access-code")] string GitHubAccessCode,
[property: CliOption("--git-hub-access-token-base-url")] string GitHubAccessTokenBaseUrl
) : AzOptions
{
    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--git-hub-client-id")]
    public string? GitHubClientId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}