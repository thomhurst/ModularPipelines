using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "config-server", "git", "repo", "add")]
public record AzSpringCloudConfigServerGitRepoAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--repo-name")] string RepoName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--host-key")]
    public string? HostKey { get; set; }

    [CommandSwitch("--host-key-algorithm")]
    public string? HostKeyAlgorithm { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--pattern")]
    public string? Pattern { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }

    [CommandSwitch("--search-paths")]
    public string? SearchPaths { get; set; }

    [CommandSwitch("--strict-host-key-checking")]
    public string? StrictHostKeyChecking { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}