using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("show-ref")]
[ExcludeFromCodeCoverage]
public record GitShowRefOptions : GitOptions
{
    [BooleanCommandSwitch("--head")]
    public virtual bool? Head { get; set; }

    [BooleanCommandSwitch("--heads")]
    public virtual bool? Heads { get; set; }

    [BooleanCommandSwitch("--tags")]
    public virtual bool? Tags { get; set; }

    [BooleanCommandSwitch("--dereference")]
    public virtual bool? Dereference { get; set; }

    [CommandEqualsSeparatorSwitch("--hash")]
    public string? Hash { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude-existing")]
    public string? ExcludeExisting { get; set; }
}