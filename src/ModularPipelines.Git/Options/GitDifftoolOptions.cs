using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("difftool")]
[ExcludeFromCodeCoverage]
public record GitDifftoolOptions : GitOptions
{
    [BooleanCommandSwitch("--dir-diff")]
    public bool? DirDiff { get; set; }

    [BooleanCommandSwitch("--no-prompt")]
    public bool? NoPrompt { get; set; }

    [BooleanCommandSwitch("--prompt")]
    public bool? Prompt { get; set; }

    [CommandEqualsSeparatorSwitch("--rotate-to")]
    public string? RotateTo { get; set; }

    [CommandEqualsSeparatorSwitch("--skip-to")]
    public string? SkipTo { get; set; }

    [CommandEqualsSeparatorSwitch("--tool")]
    public string? GitTool { get; set; }

    [BooleanCommandSwitch("--tool-help")]
    public bool? ToolHelp { get; set; }

    [BooleanCommandSwitch("--no-symlinks")]
    public bool? NoSymlinks { get; set; }

    [BooleanCommandSwitch("--symlinks")]
    public bool? Symlinks { get; set; }

    [CommandEqualsSeparatorSwitch("--extcmd")]
    public string? Extcmd { get; set; }

    [BooleanCommandSwitch("--no-gui")]
    public bool? NoGui { get; set; }

    [BooleanCommandSwitch("--gui")]
    public bool? Gui { get; set; }

    [BooleanCommandSwitch("--no-trust-exit-code")]
    public bool? NoTrustExitCode { get; set; }

    [BooleanCommandSwitch("--trust-exit-code")]
    public bool? TrustExitCode { get; set; }
}
