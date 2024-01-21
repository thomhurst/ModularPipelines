using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerStackRmOptions : DockerOptions
{
    public DockerStackRmOptions(
        IEnumerable<string> stack
    )
    {
        CommandParts = ["stack", "rm"];

        Stack = stack;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Stack { get; set; }
}
