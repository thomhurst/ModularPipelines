using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "config-server", "git", "repo", "remove")]
public record AzSpringConfigServerGitRepoRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--repo-name")] string RepoName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }
}