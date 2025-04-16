using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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
    public virtual string? Description { get; set; }

    [CommandSwitch("--docker")]
    public virtual string? Docker { get; set; }
}
