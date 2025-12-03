using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("reset")]
[ExcludeFromCodeCoverage]
public record GitResetOptions : GitOptions
{
    [CliFlag("--soft")]
    public virtual bool? Soft { get; set; }

    [CliFlag("--mixed")]
    public virtual bool? Mixed { get; set; }

    [CliFlag("--hard")]
    public virtual bool? Hard { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliFlag("--keep")]
    public virtual bool? Keep { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--refresh")]
    public virtual bool? Refresh { get; set; }

    [CliFlag("--no-refresh")]
    public virtual bool? NoRefresh { get; set; }

    [CliOption("--pathspec-from-file", Format = OptionFormat.EqualsSeparated)]
    public string? PathspecFromFile { get; set; }

    [CliFlag("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}