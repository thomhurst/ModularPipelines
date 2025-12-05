using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("ls-files")]
[ExcludeFromCodeCoverage]
public record GitLsFilesOptions : GitOptions
{
    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliFlag("--deleted")]
    public virtual bool? Deleted { get; set; }

    [CliFlag("--modified")]
    public virtual bool? Modified { get; set; }

    [CliFlag("--others")]
    public virtual bool? Others { get; set; }

    [CliFlag("--ignored")]
    public virtual bool? Ignored { get; set; }

    [CliFlag("--stage")]
    public virtual bool? Stage { get; set; }

    [CliFlag("--directory")]
    public virtual bool? Directory { get; set; }

    [CliFlag("--no-empty-directory")]
    public virtual bool? NoEmptyDirectory { get; set; }

    [CliFlag("--unmerged")]
    public virtual bool? Unmerged { get; set; }

    [CliFlag("--killed")]
    public virtual bool? Killed { get; set; }

    [CliFlag("--resolve-undo")]
    public virtual bool? ResolveUndo { get; set; }

    [CliFlag("--deduplicate")]
    public virtual bool? Deduplicate { get; set; }

    [CliOption("--exclude", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Exclude { get; set; }

    [CliOption("--exclude-from", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ExcludeFrom { get; set; }

    [CliOption("--exclude-per-directory", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ExcludePerDirectory { get; set; }

    [CliFlag("--exclude-standard")]
    public virtual bool? ExcludeStandard { get; set; }

    [CliFlag("--error-unmatch")]
    public virtual bool? ErrorUnmatch { get; set; }

    [CliOption("--with-tree", Format = OptionFormat.EqualsSeparated)]
    public virtual string? WithTree { get; set; }

    [CliFlag("--full-name")]
    public virtual bool? FullName { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Abbrev { get; set; }

    [CliFlag("--debug")]
    public virtual bool? Debug { get; set; }

    [CliFlag("--eol")]
    public virtual bool? Eol { get; set; }

    [CliFlag("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Format { get; set; }
}