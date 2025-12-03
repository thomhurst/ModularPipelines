using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cache")]
public record CacheOptions : ChocoOptions
{
    [CliFlag("--expired")]
    public virtual bool? Expired { get; set; }
}