using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("ls-files")]
[ExcludeFromCodeCoverage]
public record GitLsFilesOptions : GitOptions
{
    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public virtual bool? Deleted { get; set; }

    [BooleanCommandSwitch("--modified")]
    public virtual bool? Modified { get; set; }

    [BooleanCommandSwitch("--others")]
    public virtual bool? Others { get; set; }

    [BooleanCommandSwitch("--ignored")]
    public virtual bool? Ignored { get; set; }

    [BooleanCommandSwitch("--stage")]
    public virtual bool? Stage { get; set; }

    [BooleanCommandSwitch("--directory")]
    public virtual bool? Directory { get; set; }

    [BooleanCommandSwitch("--no-empty-directory")]
    public virtual bool? NoEmptyDirectory { get; set; }

    [BooleanCommandSwitch("--unmerged")]
    public virtual bool? Unmerged { get; set; }

    [BooleanCommandSwitch("--killed")]
    public virtual bool? Killed { get; set; }

    [BooleanCommandSwitch("--resolve-undo")]
    public virtual bool? ResolveUndo { get; set; }

    [BooleanCommandSwitch("--deduplicate")]
    public virtual bool? Deduplicate { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude-from")]
    public string? ExcludeFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude-per-directory")]
    public string? ExcludePerDirectory { get; set; }

    [BooleanCommandSwitch("--exclude-standard")]
    public virtual bool? ExcludeStandard { get; set; }

    [BooleanCommandSwitch("--error-unmatch")]
    public virtual bool? ErrorUnmatch { get; set; }

    [CommandEqualsSeparatorSwitch("--with-tree")]
    public string? WithTree { get; set; }

    [BooleanCommandSwitch("--full-name")]
    public virtual bool? FullName { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--debug")]
    public virtual bool? Debug { get; set; }

    [BooleanCommandSwitch("--eol")]
    public virtual bool? Eol { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }
}