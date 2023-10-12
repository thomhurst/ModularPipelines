using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("version", "check")]
public record YarnVersionCheckOptions : YarnOptions
{
    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }
}