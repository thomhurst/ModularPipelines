using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret create")]
public record DockerSecretCreateOptions : DockerOptions
{
    [CommandLongSwitch("driver")]
    public string? Driver { get; set; }

    [CommandLongSwitch("template-driver")]
    public string? TemplateDriver { get; set; }

}