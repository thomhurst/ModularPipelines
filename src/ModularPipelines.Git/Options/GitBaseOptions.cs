using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[ExcludeFromCodeCoverage]
public record GitBaseOptions : GitOptions
{
    [BooleanCommandSwitch("--version")] public bool? Version { get; set; }

    [CommandEqualsSeparatorSwitch("--config-env")] public string[]? ConfigEnv { get; set; }

    [CommandEqualsSeparatorSwitch("--exec-path")] public string? ExecPath { get; set; }

    [BooleanCommandSwitch("--html-path")] public bool? HtmlPath { get; set; }

    [BooleanCommandSwitch("--man-path")] public bool? ManPath { get; set; }

    [BooleanCommandSwitch("--info-path")] public bool? InfoPath { get; set; }

    [BooleanCommandSwitch("--paginate")] public bool? Paginate { get; set; }

    [BooleanCommandSwitch("--no-pager")] public bool? NoPager { get; set; }

    [CommandEqualsSeparatorSwitch("--git-dir")] public string? GitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--work-tree")] public string? WorkTree { get; set; }

    [CommandEqualsSeparatorSwitch("--namespace")] public string? Namespace { get; set; }

    [BooleanCommandSwitch("--bare")] public bool? Bare { get; set; }

    [BooleanCommandSwitch("--no-replace-objects")]
    public bool? NoReplaceObjects { get; set; }

    [BooleanCommandSwitch("--literal-pathspecs")]
    public bool? LiteralPathspecs { get; set; }

    [BooleanCommandSwitch("--glob-pathspecs")]
    public bool? GlobPathspecs { get; set; }

    [BooleanCommandSwitch("--noglob-pathspecs")]
    public bool? NoglobPathspecs { get; set; }

    [BooleanCommandSwitch("--icase-pathspecs")]
    public bool? IcasePathspecs { get; set; }

    [BooleanCommandSwitch("--no-optional-locks")]
    public bool? NoOptionalLocks { get; set; }

    [BooleanCommandSwitch("--list-cmds")] public bool? ListCmds { get; set; }

    [CommandEqualsSeparatorSwitch("--attr-source")] public string? AttrSource { get; set; }
}
