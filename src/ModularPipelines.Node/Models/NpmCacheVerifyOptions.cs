using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache", "verify")]
public record NpmCacheVerifyOptions : NpmOptions
{
    [CommandSwitch("--cache")]
    public virtual string? Cache { get; set; }
}