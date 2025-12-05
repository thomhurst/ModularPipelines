using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("stash")]
[ExcludeFromCodeCoverage]
public record GitStashOptions : GitOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--include-untracked")]
    public virtual bool? IncludeUntracked { get; set; }

    [CliFlag("--no-include-untracked")]
    public virtual bool? NoIncludeUntracked { get; set; }

    [CliFlag("--only-untracked")]
    public virtual bool? OnlyUntracked { get; set; }

    [CliFlag("--index")]
    public virtual bool? Index { get; set; }

    [CliFlag("--keep-index")]
    public virtual bool? KeepIndex { get; set; }

    [CliFlag("--no-keep-index")]
    public virtual bool? NoKeepIndex { get; set; }

    [CliFlag("--patch")]
    public virtual bool? Patch { get; set; }

    [CliFlag("--staged")]
    public virtual bool? Staged { get; set; }

    [CliOption("--pathspec-from-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? PathspecFromFile { get; set; }

    [CliFlag("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}