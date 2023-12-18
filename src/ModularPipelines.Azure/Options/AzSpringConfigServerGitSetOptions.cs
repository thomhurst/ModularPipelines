using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "config-server", "git", "set")]
public record AzSpringConfigServerGitSetOptions(
[property: CommandSwitch("--name")] string Name,
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

    [CommandSwitch("--host-key-check")]
    public string? HostKeyCheck { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }

    [CommandSwitch("--search-paths")]
    public string? SearchPaths { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}