using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "config-server", "git", "repo", "update")]
public record AzSpringConfigServerGitRepoUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--repo-name")] string RepoName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--host-key")]
    public string? HostKey { get; set; }

    [CliOption("--host-key-algorithm")]
    public string? HostKeyAlgorithm { get; set; }

    [CliOption("--host-key-check")]
    public string? HostKeyCheck { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--pattern")]
    public string? Pattern { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }

    [CliOption("--search-paths")]
    public string? SearchPaths { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}