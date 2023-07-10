using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config create")]
public record DockerConfigCreateOptions : DockerOptions
{
    [CommandLongSwitch("template-driver")]
    public string? TemplateDriver { get; set; }

}