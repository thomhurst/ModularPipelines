using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stage")]
public record YarnStageOptions : YarnOptions
{
    [BooleanCommandSwitch("--commit")]
    public bool? Commit { get; set; }

    [BooleanCommandSwitch("--reset")]
    public bool? Reset { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}