using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("add")]
public record YarnAddOptions : YarnOptions
{
    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliFlag("--fixed")]
    public virtual bool? Fixed { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliFlag("--tilde")]
    public virtual bool? Tilde { get; set; }

    [CliFlag("--caret")]
    public virtual bool? Caret { get; set; }

    [CliFlag("--dev")]
    public virtual bool? Dev { get; set; }

    [CliFlag("--peer")]
    public virtual bool? Peer { get; set; }

    [CliFlag("--optional")]
    public virtual bool? Optional { get; set; }

    [CliFlag("--prefer-dev")]
    public virtual bool? PreferDev { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliOption("--mode")]
    public virtual string? Mode { get; set; }
}