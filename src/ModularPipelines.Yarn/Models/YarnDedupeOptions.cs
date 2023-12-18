using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dedupe")]
public record YarnDedupeOptions : YarnOptions
{
    [CommandSwitch("--strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("--check")]
    public bool? Check { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }
}