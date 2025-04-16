using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dedupe")]
public record YarnDedupeOptions : YarnOptions
{
    [CommandSwitch("--strategy")]
    public virtual string? Strategy { get; set; }

    [BooleanCommandSwitch("--check")]
    public virtual bool? Check { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [CommandSwitch("--mode")]
    public virtual string? Mode { get; set; }
}