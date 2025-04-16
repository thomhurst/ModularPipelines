using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("doctor")]
public record NpmDoctorOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Ping { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Versions { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Environment { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Permissions { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Cache { get; set; }
}