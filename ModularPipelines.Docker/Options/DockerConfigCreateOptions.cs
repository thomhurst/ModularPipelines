using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config create")]
public record DockerConfigCreateOptions([property: PositionalArgument(Position = Position.AfterArguments)] string Config) : DockerOptions
{

    [CommandSwitch("--template-driver")]
    public string? TemplateDriver { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

}