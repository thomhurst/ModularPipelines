using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "get-git-hub-access-token")]
public record AzDatafactoryGetGitHubAccessTokenOptions(
[property: CommandSwitch("--git-hub-access-code")] string GitHubAccessCode,
[property: CommandSwitch("--git-hub-access-token-base-url")] string GitHubAccessTokenBaseUrl
) : AzOptions
{
    [CommandSwitch("--factory-name")]
    public string? FactoryName { get; set; }

    [CommandSwitch("--git-hub-client-id")]
    public string? GitHubClientId { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}