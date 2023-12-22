using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("secret create")]
[ExcludeFromCodeCoverage]
public record DockerSecretCreateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Secret) : DockerOptions
{
    [CommandSwitch("--driver")]
    public string? Driver { get; set; }

    [CommandSwitch("--template-driver")]
    public string? TemplateDriver { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }
}