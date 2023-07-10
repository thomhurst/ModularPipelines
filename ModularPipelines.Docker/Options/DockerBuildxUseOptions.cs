using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx use")]
public record DockerBuildxUseOptions : DockerOptions
{
    [CommandLongSwitch("default")]
    public string? Default { get; set; }

    [CommandLongSwitch("global")]
    public string? Global { get; set; }

}