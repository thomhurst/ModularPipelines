using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("cache", "clean")]
public record YarnCacheCleanOptions : YarnOptions
{
    [CliFlag("--mirror")]
    public virtual bool? Mirror { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }
}