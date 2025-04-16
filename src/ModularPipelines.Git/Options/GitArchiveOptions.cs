using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("archive")]
[ExcludeFromCodeCoverage]
public record GitArchiveOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandEqualsSeparatorSwitch("--output")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--add-file")]
    public string? AddFile { get; set; }

    [CommandEqualsSeparatorSwitch("--add-virtual-file")]
    public string? AddVirtualFile { get; set; }

    [BooleanCommandSwitch("--worktree-attributes")]
    public virtual bool? WorktreeAttributes { get; set; }

    [CommandEqualsSeparatorSwitch("--mtime")]
    public string? Mtime { get; set; }

    [CommandEqualsSeparatorSwitch("--remote")]
    public string? Remote { get; set; }

    [CommandEqualsSeparatorSwitch("--exec")]
    public string? Exec { get; set; }
}