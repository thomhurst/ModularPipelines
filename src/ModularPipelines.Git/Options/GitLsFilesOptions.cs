using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("ls-files")]
public record GitLsFilesOptions : GitOptions
{
    [BooleanCommandSwitch("--c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--deleted")]
    public bool? Deleted { get; set; }

    [BooleanCommandSwitch("--modified")]
    public bool? Modified { get; set; }

    [BooleanCommandSwitch("--others")]
    public bool? Others { get; set; }

    [BooleanCommandSwitch("--ignored")]
    public bool? Ignored { get; set; }

    [BooleanCommandSwitch("--stage")]
    public bool? Stage { get; set; }

    [BooleanCommandSwitch("--directory")]
    public bool? Directory { get; set; }

    [BooleanCommandSwitch("--no-empty-directory")]
    public bool? NoEmptyDirectory { get; set; }

    [BooleanCommandSwitch("--unmerged")]
    public bool? Unmerged { get; set; }

    [BooleanCommandSwitch("--killed")]
    public bool? Killed { get; set; }

    [BooleanCommandSwitch("--resolve-undo")]
    public bool? ResolveUndo { get; set; }

    [BooleanCommandSwitch("--deduplicate")]
    public bool? Deduplicate { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude-from")]
    public string? ExcludeFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude-per-directory")]
    public string? ExcludePerDirectory { get; set; }

    [BooleanCommandSwitch("--exclude-standard")]
    public bool? ExcludeStandard { get; set; }

    [BooleanCommandSwitch("--error-unmatch")]
    public bool? ErrorUnmatch { get; set; }

    [CommandEqualsSeparatorSwitch("--with-tree")]
    public string? WithTree { get; set; }

    [BooleanCommandSwitch("--full-name")]
    public bool? FullName { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [BooleanCommandSwitch("--eol")]
    public bool? Eol { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public bool? Sparse { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }
}