using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("ls-tree")]
[ExcludeFromCodeCoverage]
public record GitLsTreeOptions : GitOptions
{
    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--name-status")]
    public bool? NameStatus { get; set; }

    [BooleanCommandSwitch("--object-only")]
    public bool? ObjectOnly { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--full-name")]
    public bool? FullName { get; set; }

    [BooleanCommandSwitch("--full-tree")]
    public bool? FullTree { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }
}