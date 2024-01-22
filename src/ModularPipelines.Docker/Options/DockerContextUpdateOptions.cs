using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerContextUpdateOptions : DockerOptions
{
    public DockerContextUpdateOptions(
        string context
    )
    {
        CommandParts = ["context", "update"];

        UpdateContext = context;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? UpdateContext { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--docker")]
    public string? Docker { get; set; }
}
