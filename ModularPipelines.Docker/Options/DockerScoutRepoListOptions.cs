using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout repo list")]
public record DockerScoutRepoListOptions : DockerOptions
{
    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [CommandLongSwitch("only-disabled")]
    public string? OnlyDisabled { get; set; }

    [CommandLongSwitch("only-enabled")]
    public string? OnlyEnabled { get; set; }

}