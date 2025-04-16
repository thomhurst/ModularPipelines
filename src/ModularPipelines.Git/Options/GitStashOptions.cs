using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("stash")]
[ExcludeFromCodeCoverage]
public record GitStashOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--include-untracked")]
    public virtual bool? IncludeUntracked { get; set; }

    [BooleanCommandSwitch("--no-include-untracked")]
    public virtual bool? NoIncludeUntracked { get; set; }

    [BooleanCommandSwitch("--only-untracked")]
    public virtual bool? OnlyUntracked { get; set; }

    [BooleanCommandSwitch("--index")]
    public virtual bool? Index { get; set; }

    [BooleanCommandSwitch("--keep-index")]
    public virtual bool? KeepIndex { get; set; }

    [BooleanCommandSwitch("--no-keep-index")]
    public virtual bool? NoKeepIndex { get; set; }

    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [BooleanCommandSwitch("--staged")]
    public virtual bool? Staged { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}