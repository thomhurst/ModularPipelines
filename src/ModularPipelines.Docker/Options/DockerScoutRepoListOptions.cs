using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "repo", "list")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoListOptions : DockerOptions
{
    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--only-disabled")]
    public virtual string? OnlyDisabled { get; set; }

    [CommandSwitch("--only-enabled")]
    public virtual string? OnlyEnabled { get; set; }

    [CommandSwitch("--only-registry")]
    public virtual string? OnlyRegistry { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }
}
