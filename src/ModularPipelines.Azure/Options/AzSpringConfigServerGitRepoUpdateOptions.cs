using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "config-server", "git", "repo", "update")]
public record AzSpringConfigServerGitRepoUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--repo-name")] string RepoName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--host-key")]
    public string? HostKey { get; set; }

    [CommandSwitch("--host-key-algorithm")]
    public string? HostKeyAlgorithm { get; set; }

    [CommandSwitch("--host-key-check")]
    public string? HostKeyCheck { get; set; }

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

    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}