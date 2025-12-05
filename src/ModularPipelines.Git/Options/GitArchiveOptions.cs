using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("archive")]
[ExcludeFromCodeCoverage]
public record GitArchiveOptions : GitOptions
{
    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Format { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliOption("--prefix", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Prefix { get; set; }

    [CliOption("--output", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Output { get; set; }

    [CliOption("--add-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? AddFile { get; set; }

    [CliOption("--add-virtual-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? AddVirtualFile { get; set; }

    [CliFlag("--worktree-attributes")]
    public virtual bool? WorktreeAttributes { get; set; }

    [CliOption("--mtime", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Mtime { get; set; }

    [CliOption("--remote", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Remote { get; set; }

    [CliOption("--exec", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Exec { get; set; }
}