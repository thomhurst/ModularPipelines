using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stage")]
public record YarnStageOptions : YarnOptions
{
    [BooleanCommandSwitch("--commit")]
    public virtual bool? Commit { get; set; }

    [BooleanCommandSwitch("--reset")]
    public virtual bool? Reset { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }
}