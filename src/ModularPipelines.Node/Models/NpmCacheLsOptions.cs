using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache", "ls")]
public record NpmCacheLsOptions : NpmOptions
{
    [CommandSwitch("--cache")]
    public string? Cache { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Name { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Version { get; set; }
}