using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("difftool")]
[ExcludeFromCodeCoverage]
public record GitDifftoolOptions : GitOptions
{
    [BooleanCommandSwitch("--dir-diff")]
    public virtual bool? DirDiff { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public virtual bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("--prompt")]
    public virtual bool? Prompt { get; set; }

    [CommandEqualsSeparatorSwitch("--rotate-to")]
    public string? RotateTo { get; set; }

    [CommandEqualsSeparatorSwitch("--skip-to")]
    public string? SkipTo { get; set; }

    [CommandEqualsSeparatorSwitch("--tool")]
    public string? GitTool { get; set; }

    [BooleanCommandSwitch("--tool-help")]
    public virtual bool? ToolHelp { get; set; }

    [BooleanCommandSwitch("--no-symlinks")]
    public virtual bool? NoSymlinks { get; set; }

    [BooleanCommandSwitch("--symlinks")]
    public virtual bool? Symlinks { get; set; }

    [CommandEqualsSeparatorSwitch("--extcmd")]
    public string? Extcmd { get; set; }

    [BooleanCommandSwitch("--no-gui")]
    public virtual bool? NoGui { get; set; }

    [BooleanCommandSwitch("--gui")]
    public virtual bool? Gui { get; set; }

    [BooleanCommandSwitch("--no-trust-exit-code")]
    public virtual bool? NoTrustExitCode { get; set; }

    [BooleanCommandSwitch("--trust-exit-code")]
    public virtual bool? TrustExitCode { get; set; }
}