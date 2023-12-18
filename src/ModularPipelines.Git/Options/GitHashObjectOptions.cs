using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("hash-object")]
[ExcludeFromCodeCoverage]
public record GitHashObjectOptions : GitOptions
{
    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--stdin-paths")]
    public bool? StdinPaths { get; set; }

    [BooleanCommandSwitch("--path")]
    public bool? Path { get; set; }

    [BooleanCommandSwitch("--no-filters")]
    public bool? NoFilters { get; set; }

    [BooleanCommandSwitch("--literally")]
    public bool? Literally { get; set; }
}