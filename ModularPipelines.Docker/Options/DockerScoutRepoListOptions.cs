using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout repo list")]
public record DockerScoutRepoListOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Org) : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--only-disabled")]
    public string? OnlyDisabled { get; set; }

    [CommandSwitch("--only-enabled")]
    public string? OnlyEnabled { get; set; }

}