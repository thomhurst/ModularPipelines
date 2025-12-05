using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cache", "remove")]
public record CacheRemoveOptions : ChocoOptions
{
    [CliFlag("--expired")]
    public virtual bool? Expired { get; set; }
}