using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout repo list")]
[ExcludeFromCodeCoverage]
public record DockerScoutRepoListOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Org) : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--only-disabled")]
    public string? OnlyDisabled { get; set; }

    [CommandSwitch("--only-enabled")]
    public string? OnlyEnabled { get; set; }
}
