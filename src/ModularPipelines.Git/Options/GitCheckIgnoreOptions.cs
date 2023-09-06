using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("check-ignore")]
[ExcludeFromCodeCoverage]
public record GitCheckIgnoreOptions : GitOptions
{
    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--no-index")]
    public bool? NoIndex { get; set; }
}
