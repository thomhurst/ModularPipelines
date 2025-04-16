using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerBuildxUseOptions : DockerOptions
{
    public DockerBuildxUseOptions(
        string name
    )
    {
        CommandParts = ["buildx", "use"];

        Name = name;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Name { get; set; }

    [CommandSwitch("--default")]
    public virtual string? Default { get; set; }

    [CommandSwitch("--global")]
    public virtual string? Global { get; set; }
}
