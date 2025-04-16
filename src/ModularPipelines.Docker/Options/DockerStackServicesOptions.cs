using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerStackServicesOptions : DockerOptions
{
    public DockerStackServicesOptions(
        string stack
    )
    {
        CommandParts = ["stack", "services"];

        Stack = stack;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Stack { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
