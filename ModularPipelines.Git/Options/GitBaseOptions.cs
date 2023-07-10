using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

public record GitBaseOptions : GitOptions
{
    [BooleanCommandSwitch("version")] public bool? Version { get; set; }

    [CommandLongSwitch("config-env")] public string[]? ConfigEnv { get; set; }

    [CommandLongSwitch("exec-path")] public string? ExecPath { get; set; }

    [BooleanCommandSwitch("html-path")] public bool? HtmlPath { get; set; }

    [BooleanCommandSwitch("man-path")] public bool? ManPath { get; set; }

    [BooleanCommandSwitch("info-path")] public bool? InfoPath { get; set; }

    [BooleanCommandSwitch("paginate")] public bool? Paginate { get; set; }

    [BooleanCommandSwitch("no-pager")] public bool? NoPager { get; set; }

    [CommandLongSwitch("git-dir")] public string? GitDir { get; set; }

    [CommandLongSwitch("work-tree")] public string? WorkTree { get; set; }

    [CommandLongSwitch("namespace")] public string? Namespace { get; set; }

    [BooleanCommandSwitch("bare")] public bool? Bare { get; set; }

    [BooleanCommandSwitch("no-replace-objects")]
    public bool? NoReplaceObjects { get; set; }

    [BooleanCommandSwitch("literal-pathspecs")]
    public bool? LiteralPathspecs { get; set; }

    [BooleanCommandSwitch("glob-pathspecs")]
    public bool? GlobPathspecs { get; set; }

    [BooleanCommandSwitch("noglob-pathspecs")]
    public bool? NoglobPathspecs { get; set; }

    [BooleanCommandSwitch("icase-pathspecs")]
    public bool? IcasePathspecs { get; set; }

    [BooleanCommandSwitch("no-optional-locks")]
    public bool? NoOptionalLocks { get; set; }

    [BooleanCommandSwitch("list-cmds")] public bool? ListCmds { get; set; }

    [CommandLongSwitch("attr-source")] public string? AttrSource { get; set; }

}