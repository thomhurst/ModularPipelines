using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("init")]
[ExcludeFromCodeCoverage]
public record GitInitOptions : GitOptions
{
    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--bare")]
    public virtual bool? Bare { get; set; }

    [CommandEqualsSeparatorSwitch("--object-format")]
    public string? ObjectFormat { get; set; }

    [CommandEqualsSeparatorSwitch("--template")]
    public string? Template { get; set; }

    [CommandEqualsSeparatorSwitch("--separate-git-dir")]
    public string? SeparateGitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--initial-branch")]
    public string? InitialBranch { get; set; }

    [CommandEqualsSeparatorSwitch("--shared")]
    public string? Shared { get; set; }
}