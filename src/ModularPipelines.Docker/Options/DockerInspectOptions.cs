using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerInspectOptions : DockerOptions
{
    public DockerInspectOptions(
        IEnumerable<string> nameOrId
    )
    {
        CommandParts = ["inspect"];

        NameOrId = nameOrId;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? NameOrId { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--size")]
    public virtual string? Size { get; set; }

    [CommandSwitch("--type")]
    public virtual string? Type { get; set; }
}
