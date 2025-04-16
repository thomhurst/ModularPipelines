using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerConfigCreateOptions : DockerOptions
{
    public DockerConfigCreateOptions(
        string config,
        string file
    )
    {
        CommandParts = ["config", "create"];

        CreateConfig = config;

        File = file;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? CreateConfig { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? File { get; set; }

    [CommandSwitch("--label")]
    public virtual string? Label { get; set; }

    [CommandSwitch("--template-driver")]
    public virtual string? TemplateDriver { get; set; }
}
