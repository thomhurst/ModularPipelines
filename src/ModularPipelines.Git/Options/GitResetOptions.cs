using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("reset")]
[ExcludeFromCodeCoverage]
public record GitResetOptions : GitOptions
{
    [BooleanCommandSwitch("--soft")]
    public bool? Soft { get; set; }

    [BooleanCommandSwitch("--mixed")]
    public bool? Mixed { get; set; }

    [BooleanCommandSwitch("--hard")]
    public bool? Hard { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [BooleanCommandSwitch("--keep")]
    public bool? Keep { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--refresh")]
    public bool? Refresh { get; set; }

    [BooleanCommandSwitch("--no-refresh")]
    public bool? NoRefresh { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }
}