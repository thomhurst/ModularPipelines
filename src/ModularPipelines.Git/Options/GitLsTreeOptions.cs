using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("ls-tree")]
[ExcludeFromCodeCoverage]
public record GitLsTreeOptions : GitOptions
{
    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--name-status")]
    public virtual bool? NameStatus { get; set; }

    [BooleanCommandSwitch("--object-only")]
    public virtual bool? ObjectOnly { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--full-name")]
    public virtual bool? FullName { get; set; }

    [BooleanCommandSwitch("--full-tree")]
    public virtual bool? FullTree { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }
}