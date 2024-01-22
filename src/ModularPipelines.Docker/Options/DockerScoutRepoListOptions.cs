using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "repo", "list")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoListOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--only-disabled")]
    public string? OnlyDisabled { get; set; }

    [CommandSwitch("--only-enabled")]
    public string? OnlyEnabled { get; set; }

    [CommandSwitch("--only-registry")]
    public string? OnlyRegistry { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}
