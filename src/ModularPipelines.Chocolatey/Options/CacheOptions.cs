using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache")]
public record CacheOptions : ChocoOptions
{
    [BooleanCommandSwitch("--expired")]
    public bool? Expired { get; set; }
}