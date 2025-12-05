using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Stack { get; set; }
}
