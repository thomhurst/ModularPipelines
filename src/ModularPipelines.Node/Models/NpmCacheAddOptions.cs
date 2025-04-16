using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache", "add")]
public record NpmCacheAddOptions : NpmOptions
{
    [CommandSwitch("--cache")]
    public virtual string? Cache { get; set; }
}