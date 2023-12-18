using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "config-server", "git", "repo", "remove")]
public record AzSpringCloudConfigServerGitRepoRemoveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--repo-name")] string RepoName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }
}