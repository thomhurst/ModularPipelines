using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("show-ref")]
[ExcludeFromCodeCoverage]
public record GitShowRefOptions : GitOptions
{
    [BooleanCommandSwitch("--head")]
    public bool? Head { get; set; }

    [BooleanCommandSwitch("--heads")]
    public bool? Heads { get; set; }

    [BooleanCommandSwitch("--tags")]
    public bool? Tags { get; set; }

    [BooleanCommandSwitch("--dereference")]
    public bool? Dereference { get; set; }

    [CommandEqualsSeparatorSwitch("--hash")]
    public string? Hash { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude-existing")]
    public string? ExcludeExisting { get; set; }
}