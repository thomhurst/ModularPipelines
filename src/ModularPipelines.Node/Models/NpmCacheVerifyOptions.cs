using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("cache", "verify")]
public record NpmCacheVerifyOptions : NpmOptions
{
    [CliOption("--cache")]
    public virtual string? Cache { get; set; }
}