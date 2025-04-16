using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("add")]
public record YarnAddOptions : YarnOptions
{
    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--fixed")]
    public virtual bool? Fixed { get; set; }

    [BooleanCommandSwitch("--exact")]
    public virtual bool? Exact { get; set; }

    [BooleanCommandSwitch("--tilde")]
    public virtual bool? Tilde { get; set; }

    [BooleanCommandSwitch("--caret")]
    public virtual bool? Caret { get; set; }

    [BooleanCommandSwitch("--dev")]
    public virtual bool? Dev { get; set; }

    [BooleanCommandSwitch("--peer")]
    public virtual bool? Peer { get; set; }

    [BooleanCommandSwitch("--optional")]
    public virtual bool? Optional { get; set; }

    [BooleanCommandSwitch("--prefer-dev")]
    public virtual bool? PreferDev { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [CommandSwitch("--mode")]
    public virtual string? Mode { get; set; }
}