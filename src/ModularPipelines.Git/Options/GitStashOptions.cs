using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("stash")]
[ExcludeFromCodeCoverage]
public record GitStashOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--include-untracked")]
    public bool? IncludeUntracked { get; set; }

    [BooleanCommandSwitch("--no-include-untracked")]
    public bool? NoIncludeUntracked { get; set; }

    [BooleanCommandSwitch("--only-untracked")]
    public bool? OnlyUntracked { get; set; }

    [BooleanCommandSwitch("--index")]
    public bool? Index { get; set; }

    [BooleanCommandSwitch("--keep-index")]
    public bool? KeepIndex { get; set; }

    [BooleanCommandSwitch("--no-keep-index")]
    public bool? NoKeepIndex { get; set; }

    [BooleanCommandSwitch("--patch")]
    public bool? Patch { get; set; }

    [BooleanCommandSwitch("--staged")]
    public bool? Staged { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}