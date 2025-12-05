using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("difftool")]
[ExcludeFromCodeCoverage]
public record GitDifftoolOptions : GitOptions
{
    [CliFlag("--dir-diff")]
    public virtual bool? DirDiff { get; set; }

    [CliFlag("--no-prompt")]
    public virtual bool? NoPrompt { get; set; }

    [CliFlag("--prompt")]
    public virtual bool? Prompt { get; set; }

    [CliOption("--rotate-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RotateTo { get; set; }

    [CliOption("--skip-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SkipTo { get; set; }

    [CliOption("--tool", Format = OptionFormat.EqualsSeparated)]
    public virtual string? GitTool { get; set; }

    [CliFlag("--tool-help")]
    public virtual bool? ToolHelp { get; set; }

    [CliFlag("--no-symlinks")]
    public virtual bool? NoSymlinks { get; set; }

    [CliFlag("--symlinks")]
    public virtual bool? Symlinks { get; set; }

    [CliOption("--extcmd", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Extcmd { get; set; }

    [CliFlag("--no-gui")]
    public virtual bool? NoGui { get; set; }

    [CliFlag("--gui")]
    public virtual bool? Gui { get; set; }

    [CliFlag("--no-trust-exit-code")]
    public virtual bool? NoTrustExitCode { get; set; }

    [CliFlag("--trust-exit-code")]
    public virtual bool? TrustExitCode { get; set; }
}