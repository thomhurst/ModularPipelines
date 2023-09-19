using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config create")]
[ExcludeFromCodeCoverage]
public record DockerConfigCreateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string ConfigName) : DockerOptions
{
    [CommandSwitch("--template-driver")]
    public string? TemplateDriver { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }
}
