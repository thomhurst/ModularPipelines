using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cache", "clean")]
public record YarnCacheCleanOptions : YarnOptions
{
    [BooleanCommandSwitch("--mirror")]
    public bool? Mirror { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}