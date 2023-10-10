using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache")]
public record NpmCacheOptions : NpmOptions
{
    [CommandSwitch("--cache")]
    public string? Cache { get; set; }
}