using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("add")]
public record YarnAddOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--fixed")]
    public bool? Fixed { get; set; }

    [BooleanCommandSwitch("--exact")]
    public bool? Exact { get; set; }

    [BooleanCommandSwitch("--tilde")]
    public bool? Tilde { get; set; }

    [BooleanCommandSwitch("--caret")]
    public bool? Caret { get; set; }

    [BooleanCommandSwitch("--dev")]
    public bool? Dev { get; set; }

    [BooleanCommandSwitch("--peer")]
    public bool? Peer { get; set; }

    [BooleanCommandSwitch("--optional")]
    public bool? Optional { get; set; }

    [BooleanCommandSwitch("--prefer-dev")]
    public bool? PreferDev { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }
}