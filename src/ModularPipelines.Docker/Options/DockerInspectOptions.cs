using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerInspectOptions : DockerOptions
{
    public DockerInspectOptions(
        IEnumerable<string> nameId
    )
    {
        CommandParts = ["inspect"];

        NameId = nameId;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? NameId { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}
