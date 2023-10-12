using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache", "clean")]
public record NpmCacheCleanOptions : NpmOptions
{
    [CommandSwitch("--cache")]
    public string? Cache { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Key { get; set; }
}