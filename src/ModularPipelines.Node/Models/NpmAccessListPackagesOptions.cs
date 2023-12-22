using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access", "list", "packages")]
public record NpmAccessListPackagesOptions : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? User { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Scope { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Package { get; set; }
}