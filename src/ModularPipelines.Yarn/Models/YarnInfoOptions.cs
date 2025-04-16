using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("info")]
public record YarnInfoOptions : YarnOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CommandSwitch("--extra")]
    public virtual string? Extra { get; set; }

    [BooleanCommandSwitch("--cache")]
    public virtual bool? Cache { get; set; }

    [BooleanCommandSwitch("--dependents")]
    public virtual bool? Dependents { get; set; }

    [BooleanCommandSwitch("--manifest")]
    public virtual bool? Manifest { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--virtuals")]
    public virtual bool? Virtuals { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }
}