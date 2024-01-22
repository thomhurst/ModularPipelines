using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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
    public string? Default { get; set; }

    [CommandSwitch("--global")]
    public string? Global { get; set; }
}
